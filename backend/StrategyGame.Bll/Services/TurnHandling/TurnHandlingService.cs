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

            foreach (var c in context.Countries)
            {
                var builder = await Handler.HandlePreCombatAsync(context, c.Id, cancel);
                turnData.Add(c.Id, builder);
            }

            Handler.CountryLooted += HandleLoot;
            foreach (var c in context.Countries)
            {
                await Handler.HandleCombatAsync(context, c.Id, turnData[c.Id], id => turnData[id], cancel);
            }
            Handler.CountryLooted -= HandleLoot;

            foreach (var c in context.Countries)
            {
                await Handler.HandlePostCombatAsync(context, c.Id, turnData[c.Id], cancel);
            }

            int index = 0;
            await context.Countries.OrderByDescending(c => c.Score).ForEachAsync(c => c.Rank = ++index);

            var globals = await context.GlobalValues.SingleAsync();
            globals.Round++;

            await context.SaveChangesAsync();

            // TODO Remove invalid in progress stuff
            //    context2.InProgressBuildings.RemoveRange(context.InProgressBuildings.Where(b => b.TimeLeft <= 0));
            //    context2.InProgressResearches.RemoveRange(context.InProgressResearches.Where(r => r.TimeLeft <= 0));
        }
    }
}