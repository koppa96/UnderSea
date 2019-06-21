using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.TurnHandling
{
    /// <summary>
    /// Handles turns in parts for individual countries.
    /// </summary>
    public class CountryTurnHandler
    {
        // Static random generation for attack power modification
        static readonly Random rng = new Random();

        protected ModifierParserContainer Parsers { get; }

        /// <summary>
        /// The event that is raised when a country is looted.
        /// </summary>
        public event EventHandler<CountryLootedEventArgs> CountryLooted;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryTurnHandler"/>.
        /// </summary>
        /// <param name="parsers">The <see cref="ModifierParserContainer"/> containing effect parsers used by the handler.</param>
        public CountryTurnHandler(ModifierParserContainer parsers)
        {
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <summary>
        /// Handles pre-turn claculations, like building and research completitions, and income.
        /// Returns the <see cref="CountryModifierBuilder"/> containing the modifications for the country.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="countryId">The ID of the country to handle.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>The <see cref="CountryModifierBuilder"/> containing the modifications for the country.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the specified country does not exist.</exception>
        public async Task<CountryModifierBuilder> HandlePreCombatAsync(UnderSeaDatabaseContext context, int countryId,
            CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel);

            var globals = await context.GlobalValues.SingleAsync(cancel);
            var builder = await context.ParseAllEffectForCountryAsync(countryId, Parsers);

            // #1: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier);

            // #2: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #3: Pay soldiers
            await DesertUnits(country, context, cancel);

            // #4: Advance research and buildings
            foreach (var b in country.InProgressBuildings)
            {
                if (b.TimeLeft >= 0)
                {
                    b.TimeLeft--;
                }
            }

            foreach (var r in country.InProgressResearches)
            {
                if (r.TimeLeft >= 0)
                {
                    r.TimeLeft--;
                }
            }

            // #5: Add buildings that are completed
            await context.CheckAddCompletedAsync(country.Id, cancel);

            return builder;
        }

        /// <summary>
        /// Handles combat calculations for all incoming attacks of a country. The order of the attacks is randomized.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="countryId">The ID of the country to handle.</param>
        /// <param name="builder">The <see cref="CountryModifierBuilder"/> containing the modifications of the country.</param>
        /// <param name="builderFactory">The facory that can produce builders for country IDs.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>The <see cref="Task"/> representing the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the specified country does not exist.</exception>
        public async Task HandleCombatAsync(UnderSeaDatabaseContext context, int countryId,
            CountryModifierBuilder builder, Func<int, CountryModifierBuilder> builderFactory, CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builderFactory == null)
            {
                throw new ArgumentNullException(nameof(builderFactory));
            }

            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.IncomingAttacks).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel);

            // Randomize the incoming attacks, excluding the defending forces
            var incomingAttacks = country.IncomingAttacks
                .Where(c => !c.ParentCountry.Equals(country))
                .Select(c => new { Order = rng.Next(), Command = c })
                .OrderBy(x => x.Order)
                .Select(x => x.Command)
                .ToList();

            var defenders = country.GetAllDefending();
            await context.Entry(defenders).Collection(c => c.Divisons).LoadAsync(cancel);

            foreach (var attack in incomingAttacks)
            {
                await context.Entry(attack).Collection(c => c.Divisons).LoadAsync(cancel);

                double attackPower = GetCurrentUnitPower(attack, true, builderFactory(attack.ParentCountry.Id));
                double defensePower = GetCurrentUnitPower(defenders, false, builder);

                if (attackPower > defensePower)
                {
                    CullUnits(defenders, KnownValues.UnitLossOnDefense);

                    var pearlLoot = (long)Math.Round(country.Pearls * KnownValues.LootPercentage);
                    var coralLoot = (long)Math.Round(country.Corals * KnownValues.LootPercentage);

                    country.Pearls -= pearlLoot;
                    country.Corals -= coralLoot;

                    CountryLooted?.Invoke(this, new CountryLootedEventArgs(coralLoot, pearlLoot,
                        attack.ParentCountry.Id, attack.TargetCountry.Id));
                }
                else
                {
                    CullUnits(attack, KnownValues.UnitLossOnDefense);
                }
            }
        }

        /// <summary>
        /// Handles post-combat calculations for a country, like calculating the score, and merging commands into the defense command.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="countryId">The ID of the country to handle.</param>
        /// <param name="builder">The <see cref="CountryModifierBuilder"/> containing the modifications of the country.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>The <see cref="Task"/> representing the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the specified country does not exist.</exception>
        public async Task HandlePostCombatAsync(UnderSeaDatabaseContext context, int countryId,
            CountryModifierBuilder builder, CancellationToken cancel)
        {
            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel);

            country.Pearls += builder.CurrentPearlLoot;
            country.Corals += builder.CurrentCoralLoot;

            long divisionScore = 0;
            foreach (var comm in country.Commands)
            {
                await context.Entry(comm).Collection(c => c.Divisons).LoadAsync(cancel);
                divisionScore += comm.Divisons.Sum(d => d.Count);
            }

            country.Score = (long)Math.Round(
                builder.Population * KnownValues.ScorePopulationMultiplier
                + country.Buildings.Count * KnownValues.ScoreBuildingMultiplier
                + divisionScore * KnownValues.ScoreUnitMultiplier
                + country.Researches.Count * KnownValues.ScoreResearchMultiplier);

            // Merge all attacking commands into the defense command, delete attacking commands
            var defenders = country.GetAllDefending();

            // Load units in defenders.
            foreach (var div in defenders.Divisons)
            {
                await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel);
            }

            foreach (var attack in country.Commands.Where(c => c.Id != defenders.Id).ToList())
            {
                foreach (var div in attack.Divisons)
                {
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel);

                    var existing = defenders.Divisons.SingleOrDefault(d => d.Unit.Id == div.Unit.Id);
                    if (existing == null)
                    {
                        div.ParentCommand = defenders;
                    }
                    else
                    {
                        existing.Count += div.Count;
                    }
                }

                context.Commands.Remove(attack);
            }
        }

        /// <summary>
        /// Calculate attack or defense power, based on the units in a command and their modifiers.
        /// Attack power is modified randomly by 5%.
        /// </summary>
        /// <param name="command">The command to get the combat power of.</param>
        /// <param name="doGetAttack">If attack or defense power should be calculated.</param>
        /// <param name="builder">The builder that contains the stat modifiers.</param>
        /// <returns>The combat power of the command.</returns>
        protected double GetCurrentUnitPower(Command command, bool doGetAttack, CountryModifierBuilder builder)
        {
            if (doGetAttack)
            {
                return command.Divisons.Sum(d => d.Count * d.Unit.AttackPower * builder.AttackModifier)
                    * (rng.NextDouble() / 10 + 0.95);
            }
            else
            {
                return command.Divisons.Sum(d => d.Count * d.Unit.DefensePower * builder.DefenseModifier);
            }
        }

        /// <summary>
        /// Deletes some units from all divisions of a command. Divisions must be loaded.
        /// </summary>
        /// <param name="command">The command to cull.</param>
        /// <param name="lostPercentage">The percentage of units lost.</param>
        protected void CullUnits(Command command, double lostPercentage)
        {
            foreach (var div in command.Divisons)
            {
                div.Count -= (int)Math.Round(div.Count * lostPercentage);

                if (div.Count < 0)
                {
                    div.Count = 0;
                }
            }
        }

        /// <summary>
        /// Deletes units until the supply / maintenance consumption can be satisfied by the country.
        /// Starts with the cheapest units.
        /// </summary>
        /// <param name="country">The country to delete from.</param>
        /// <param name="context">Te context to use.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>The task that represents the operation.</returns>
        protected async Task DesertUnits(Model.Entities.Country country, UnderSeaDatabaseContext context, CancellationToken cancel)
        {
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel);

            long pearlUpkeep = 0;
            long coralUpkeep = 0;
            foreach (var comm in country.Commands)
            {
                await context.Entry(comm).Collection(c => c.Divisons).LoadAsync(cancel);
                foreach (var div in comm.Divisons)
                {
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel);
                    pearlUpkeep += div.Count * div.Unit.CostPearl;
                    coralUpkeep += div.Count * div.Unit.CostCoral;
                }
            }

            if (coralUpkeep > country.Corals || pearlUpkeep > country.Pearls)
            {
                foreach (var div in country.Commands.SelectMany(c => c.Divisons))
                {
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel);
                }

                long pearlDeficit = pearlUpkeep - country.Pearls;
                long coralDeficit = coralUpkeep - country.Corals;

                foreach (var div in country.Commands.SelectMany(c => c.Divisons).OrderBy(d => d.Unit.CostPearl))
                {
                    long requiredPearlReduction = (long)Math.Ceiling((double)pearlDeficit / div.Unit.MaintenancePearl);
                    long requiredCoralReduction = (long)Math.Ceiling((double)coralDeficit / div.Unit.MaintenanceCoral);

                    int desertedAmount = (int)Math.Min(Math.Max(requiredCoralReduction, requiredPearlReduction), div.Count);

                    div.Count -= desertedAmount;

                    pearlDeficit -= desertedAmount * div.Unit.MaintenancePearl;
                    coralDeficit -= desertedAmount * div.Unit.MaintenanceCoral;

                    if (coralUpkeep <= country.Corals || pearlUpkeep <= country.Pearls)
                    {
                        break;
                    }
                }
            }

            country.Pearls -= pearlUpkeep;
            country.Corals -= coralUpkeep;
        }
    }
}