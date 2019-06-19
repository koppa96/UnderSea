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
        public async Task<long> HandleTurn(UnderSeaDatabase context, Country country, ModifierParserContainer parsers, CancellationToken cancel)
        {
            var builder = new CountryModifierBuilder();

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
                        if (!parsers.TryParse(e, builder))
                        {
                            Debug.WriteLine("Effect with name {0} could not be handled by the provided parsers.", e.Name);
                        }
                    }
                }
            }

            // Get global data
            var globals = await context.GlobalValues.SingleAsync().ConfigureAwait(false);

            // #1: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier);

            // #2: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #3: Pay soldiers
            var pearlUpkeep = country.Commands.Sum(c => c.Divisons.Sum(u => u.Count * u.Unit.CostPearl));
            var coralUpkeep = country.Commands.Sum(c => c.Divisons.Sum(u => u.Count * u.Unit.CostCoral));

            country.Pearls -= pearlUpkeep;
            country.Corals -= coralUpkeep;

            // TODO: remove soldiers if we got into the negatives

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

            // TODO: Combat

            // Calculate and return score
            return builder.Population
                + country.Buildings.Count * 50
                + country.Commands.Sum(c => c.Divisons.Sum(d => d.Count)) * 5
                + country.Researches.Count * 100;
        }
    }
}