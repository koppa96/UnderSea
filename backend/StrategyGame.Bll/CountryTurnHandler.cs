using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.DatabaseExtensions;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Diagnostics;
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



        public async Task<CountryModifierBuilder> HandleTurnBeginingAsync(UnderSeaDatabase context, Country country,
            CancellationToken cancel)
        {
            // Get global data
            var globals = await context.GlobalValues.SingleAsync().ConfigureAwait(false);

            // Set up builder
            var builder = new CountryModifierBuilder
            {
                BarrackSpace = globals.StartingBarrackSpace,
                Population = globals.StartingPopulation
            };

            // TODO optimize query (do it with query from context, with an async query and joins etc.
            var effectparents = country.Buildings.Select(b => new { count = b.Count, effects = b.Building.Effects.Select(e => e.Effect) })
                .Concat(country.Researches.Select(r => new { count = r.Count, effects = r.Research.Effects.Select(e => e.Effect) })).ToList();

            // Calculate the effects
            foreach (var effectParent in effectparents)
            {
                for (int iii = 0; iii < effectParent.count; iii++)
                {
                    foreach (var e in effectParent.effects)
                    {
                        if (!Parsers.TryParse(e, builder))
                        {
                            Debug.WriteLine("Effect with name {0} could not be handled by the provided parsers.", e.Name);
                        }
                    }
                }
            }

            // #1: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier);

            // #2: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #3: Pay soldiers
            DesertUnits(country);

            // Advance research and buildings
            foreach (var b in country.InProgressBuildings)
            {
                b.TimeLeft--;
            }

            foreach (var r in country.InProgressResearches)
            {
                r.TimeLeft--;
            }

            // Add buildings that are completed
            await country.CheckAddCompletedAsync(context, cancel).ConfigureAwait(false);

            return builder;
        }

        public async Task HandleCombatAsync(UnderSeaDatabase context, Country country,
            CountryModifierBuilder builder, CancellationToken cancel)
        {
            // First off, randomize the incoming attacks, excluding the defending forces
            var incomingAttacks = country.IncomingAttacks
                .Where(c => c.ParentCountry.Equals(country))
                .Select(c => new { Order = rng.Next(), Command = c })
                .OrderBy(x => x.Order)
                .Select(x => x.Command)
                .ToList();

            // Get defenders,  Calculate the current defense power
            var defenders = country.GetAllDefending();

            foreach (var attack in incomingAttacks)
            {
                double attackPower = GetCurrentUnitPower(attack, true, builder);
                double defensePower = GetCurrentUnitPower(defenders, false, builder);

                if (attackPower > defensePower)
                {
                    // Attackers won, defender loose 10% of units, and loose 50% of current pearl and coral
                    CullUnits(defenders, 0.1);

                    // Remove the looted amounts
                    var pearlLoot = (long)Math.Round(country.Pearls * 0.5);
                    var coralLoot = (long)Math.Round(country.Corals * 0.5);

                    country.Pearls -= pearlLoot;
                    country.Corals -= coralLoot;

                    CountryLooted?.Invoke(this, new CountryLootedEventArgs(coralLoot, pearlLoot,
                        attack.ParentCountry, attack.TargetCountry));
                }
                else
                {
                    // Defenders won, attacker loose 10% of units.
                    CullUnits(attack, 0.1);
                }
            }
        }

        public async Task<long> CalculateScoreAsync(UnderSeaDatabase context, Country country,
            CountryModifierBuilder builder, CancellationToken cancel)
        {
            return builder.Population
                + country.Buildings.Count * 50
                + country.Commands.Sum(c => c.Divisons.Sum(d => d.Count)) * 5
                + country.Researches.Count * 100;
        }

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

        protected void DesertUnits(Country country)
        {
            var pearlUpkeep = country.Commands.Sum(c => c.Divisons.Sum(u => u.Count * u.Unit.CostPearl));
            var coralUpkeep = country.Commands.Sum(c => c.Divisons.Sum(u => u.Count * u.Unit.CostCoral));

            // TODO: remove soldiers if we got into the negatives

            country.Pearls -= pearlUpkeep;
            country.Corals -= coralUpkeep;
        }
    }
}