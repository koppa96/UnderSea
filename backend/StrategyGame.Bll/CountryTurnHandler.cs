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

namespace StrategyGame.Bll
{
    public class CountryTurnHandler
    {
        static readonly Random rng = new Random();

        public ModifierParserContainer Parsers { get; }

        public event EventHandler<CountryLootedEventArgs> CountryLooted;

        public CountryTurnHandler(ModifierParserContainer parsers)
        {
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        public async Task<CountryModifierBuilder> HandlePreCombatAsync(UnderSeaDatabaseContext context, int countryId,
            CancellationToken cancel)
        {
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


            // Get global data
            var globals = await context.GlobalValues.SingleAsync(cancel).ConfigureAwait(false);

            var builder = await context.ParseAllEffectForCountryAsync(countryId, Parsers).ConfigureAwait(false);

            // #1: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier);

            // #2: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #3: Pay soldiers
            await DesertUnits(country, context, cancel).ConfigureAwait(false);

            // Advance research and buildings
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

            // Add buildings that are completed
            await context.CheckAddCompletedAsync(country.Id, cancel).ConfigureAwait(false);

            return builder;
        }

        public async Task HandleCombatAsync(UnderSeaDatabaseContext context, int countryId,
            CountryModifierBuilder builder, Func<int, CountryModifierBuilder> builderFactory, CancellationToken cancel)
        {
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
                    // Attackers won, defender loose 10% of units, and loose 50% of current pearl and coral
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
                    // Defenders won, attacker loose 10% of units.
                    CullUnits(attack, KnownValues.UnitLossOnDefense);
                }
            }
        }

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

        protected double GetCurrentUnitPower(Command command, bool doGetAttack, CountryModifierBuilder builder)
        {
            // Get attack or defense power, attack power is modified randomly +-5%.
            if (doGetAttack)
            {
                return command.Divisons.Sum(d => d.Count * d.Unit.AttackPower * builder.AttackModifier) * (rng.NextDouble() / 10 + 0.95);
            }
            else
            {
                return command.Divisons.Sum(d => d.Count * d.Unit.DefensePower * builder.DefenseModifier);
            }
        }

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

        protected async Task DesertUnits(Country country, UnderSeaDatabaseContext context, CancellationToken cancel)
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