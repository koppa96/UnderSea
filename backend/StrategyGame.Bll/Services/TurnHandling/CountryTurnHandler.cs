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
            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel).ConfigureAwait(false);
            
            // Get global data, create a builder from the effects
            var globals = await context.GlobalValues.SingleAsync(cancel).ConfigureAwait(false);
            var builder = await context.ParseAllEffectForCountryAsync(countryId, Parsers).ConfigureAwait(false);

            // #1: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier);

            // #2: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #3: Pay soldiers
            await DesertUnits(country, context, cancel).ConfigureAwait(false);

            // #4: Advance research and buildings
            foreach (var b in country.InProgressBuildings.ToList())
            {
                if (b.TimeLeft >= 0)
                {
                    b.TimeLeft--;
                }
            }

            foreach (var r in country.InProgressResearches.ToList())
            {
                if (r.TimeLeft >= 0)
                {
                    r.TimeLeft--;
                }
            }

            // #5: Add buildings that are completed
            await context.CheckAddCompletedAsync(country.Id, cancel).ConfigureAwait(false);

            // Return the builder as the unit stats will be needed again.
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
            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.IncomingAttacks).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel).ConfigureAwait(false);

            // First off, randomize the incoming attacks, excluding the defending forces
            var incomingAttacks = country.IncomingAttacks
                .Where(c => !c.ParentCountry.Equals(country))
                .Select(c => new { Order = rng.Next(), Command = c })
                .OrderBy(x => x.Order)
                .Select(x => x.Command)
                .ToList();

            // Get defenders
            var defenders = country.GetAllDefending();
            await context.Entry(defenders).Collection(c => c.Divisons).LoadAsync(cancel).ConfigureAwait(false);

            foreach (var attack in incomingAttacks)
            {
                // Load the divisions, calculate attack and defense powers
                await context.Entry(attack).Collection(c => c.Divisons).LoadAsync(cancel).ConfigureAwait(false);

                double attackPower = GetCurrentUnitPower(attack, true, builderFactory(attack.ParentCountry.Id));
                double defensePower = GetCurrentUnitPower(defenders, false, builder);

                if (attackPower > defensePower)
                {
                    // Attackers won, defender loose some units, and loose some of current pearl and coral
                    CullUnits(defenders, KnownValues.UnitLossOnDefense);

                    // Remove the looted amounts
                    var pearlLoot = (long)Math.Round(country.Pearls * KnownValues.LootPercentage);
                    var coralLoot = (long)Math.Round(country.Corals * KnownValues.LootPercentage);

                    country.Pearls -= pearlLoot;
                    country.Corals -= coralLoot;

                    CountryLooted?.Invoke(this, new CountryLootedEventArgs(coralLoot, pearlLoot,
                        attack.ParentCountry.Id, attack.TargetCountry.Id));
                }
                else
                {
                    // Defenders won, attacker loose some units
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
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel).ConfigureAwait(false);

            // Add loot
            country.Pearls += builder.CurrentPearlLoot;
            country.Corals += builder.CurrentCoralLoot;

            // Calculate score
            long divisionScore = 0;
            foreach (var comm in country.Commands)
            {
                await context.Entry(comm).Collection(c => c.Divisons).LoadAsync(cancel).ConfigureAwait(false);
                divisionScore += comm.Divisons.Sum(d => d.Count);
            }

            country.Score = (long)Math.Round(
                builder.Population * KnownValues.ScorePopulationMultiplier
                + country.Buildings.Count * KnownValues.ScoreBuildingMultiplier
                + divisionScore * KnownValues.ScoreUnitMultiplier
                + country.Researches.Count * KnownValues.ScoreResearchMultiplier);

            // Merge all commands into the defense command
            var defenders = country.GetAllDefending();

            // Load units in defenders.
            foreach (var div in defenders.Divisons)
            {
                await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel).ConfigureAwait(false);
            }

            foreach (var attack in country.Commands.Where(c => c.Id != defenders.Id).ToList())
            {
                foreach (var div in attack.Divisons)
                {
                    // Load unit info in attack
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel).ConfigureAwait(false);

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
                return command.Divisons.Sum(d => d.Count * d.Unit.AttackPower * builder.AttackModifier) * (rng.NextDouble() / 10 + 0.95);
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

        protected async Task DesertUnits(Model.Entities.Country country, UnderSeaDatabaseContext context, CancellationToken cancel)
        {
            // Load commands, divisons and units in the divisions
            await context.Entry(country).Collection(c => c.Commands).LoadAsync(cancel).ConfigureAwait(false);

            long pearlUpkeep = 0;
            long coralUpkeep = 0;
            foreach (var comm in country.Commands)
            {
                await context.Entry(comm).Collection(c => c.Divisons).LoadAsync(cancel).ConfigureAwait(false);
                foreach (var div in comm.Divisons)
                {
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel).ConfigureAwait(false);
                    pearlUpkeep += div.Count * div.Unit.CostPearl;
                    coralUpkeep += div.Count * div.Unit.CostCoral;
                }
            }

            if (coralUpkeep > country.Corals || pearlUpkeep > country.Pearls)
            {
                // Load unit data
                foreach (var div in country.Commands.SelectMany(c => c.Divisons))
                {
                    await context.Entry(div).Reference(d => d.Unit).LoadAsync(cancel).ConfigureAwait(false);
                }

                long pearlDeficit = pearlUpkeep - country.Pearls;
                long coralDeficit = coralUpkeep - country.Corals;

                // Go through each div in cost order
                foreach (var div in country.Commands.SelectMany(c => c.Divisons).OrderBy(d => d.Unit.CostPearl))
                {
                    // Calculate how many units of the type in the division needs to go.
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