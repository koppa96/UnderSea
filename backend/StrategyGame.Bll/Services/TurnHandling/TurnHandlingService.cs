using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.TurnHandling
{
    public class TurnHandlingService : ITurnHandlingService
    {
        protected CountryTurnHandler Handler { get; }

        public TurnHandlingService(ModifierParserContainer parsers)
        {
            Handler = new CountryTurnHandler(parsers ?? throw new ArgumentNullException(nameof(parsers)));
        }

        public async Task EndTurnAsync(UnderSeaDatabaseContext context, CancellationToken cancel = default)
        {
            // Turndata contains the builders for all countries. The builder containes the live updated modifications
            // along with the loot acquired by the countries. When below HandleLoot method adds the looted amounts to the builder.
            var turnData = new Dictionary<int, CountryModifierBuilder>();

            void HandleLoot(object sender, CountryLootedEventArgs Args)
            {
                turnData[Args.LooterId].CurrentPearlLoot += Args.LootedPearls;
                turnData[Args.LooterId].CurrentCoralLoot += Args.LootedCorals;
            }

            // First the pre-combat is handled for all countries individually. This calculates the builders.
            foreach (var c in context.Countries)
            {
                var builder = await Handler.HandlePreCombatAsync(context, c.Id, cancel);
                turnData.Add(c.Id, builder);
            }

            // The combat happens for all countries. Loot is acquired in this phase.
            Handler.CountryLooted += HandleLoot;
            foreach (var c in context.Countries)
            {
                await Handler.HandleCombatAsync(context, c.Id, turnData[c.Id], id => turnData[id], cancel);
            }
            Handler.CountryLooted -= HandleLoot;

            // Finally post combat happens. This is where loot is applied and score calculated.
            foreach (var c in context.Countries)
            {
                await Handler.HandlePostCombatAsync(context, c.Id, turnData[c.Id], cancel);
            }

            // Calculate ranking from country scores
            int index = 0;
            await context.Countries.OrderByDescending(c => c.Score).ForEachAsync(c => c.Rank = ++index);

            // Increment turn number
            var globals = await context.GlobalValues.SingleAsync();
            globals.Round++;

            await context.SaveChangesAsync();

            // TODO Remove invalid in progress stuff
            //    context2.InProgressBuildings.RemoveRange(context.InProgressBuildings.Where(b => b.TimeLeft <= 0));
            //    context2.InProgressResearches.RemoveRange(context.InProgressResearches.Where(r => r.TimeLeft <= 0));
        }
    }
}