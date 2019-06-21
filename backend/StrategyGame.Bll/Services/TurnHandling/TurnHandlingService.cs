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
            var turnData = new Dictionary<int, CountryModifierBuilder>();

            void HandleLoot(object sender, CountryLootedEventArgs Args)
            {
                turnData[Args.LooterId].CurrentPearlLoot += Args.LootedPearls;
                turnData[Args.LooterId].CurrentCoralLoot += Args.LootedCorals;
            }

            foreach (var c in context.Countries)
            {
                var builder = await Handler.HandlePreCombatAsync(context, c.Id, cancel).ConfigureAwait(false);
                turnData.Add(c.Id, builder);
            }

            Handler.CountryLooted += HandleLoot;
            foreach (var c in context.Countries)
            {
                await Handler.HandleCombatAsync(context, c.Id, turnData[c.Id], id => turnData[id], cancel).ConfigureAwait(false);
            }
            Handler.CountryLooted -= HandleLoot;

            foreach (var c in context.Countries)
            {
                await Handler.HandlePostCombatAsync(context, c.Id, turnData[c.Id], cancel).ConfigureAwait(false);
            }

            // Calculate ranking
            int index = 0;
            await context.Countries.OrderByDescending(c => c.Score).ForEachAsync(c => c.Rank = ++index).ConfigureAwait(false);

            // Increment turn number
            var globals = await context.GlobalValues.SingleAsync().ConfigureAwait(false);
            globals.Round++;

            await context.SaveChangesAsync().ConfigureAwait(false);

            // TODO Remove invalid in progress stuff
            //foreach (var b in context.InProgressBuildings)
            //{
            //    if (b.TimeLeft < 0)
            //    {
            //        context.InProgressBuildings.Remove(b);
            //    }
            //}

            context.InProgressBuildings.RemoveRange(context.InProgressBuildings.Where(b => b.TimeLeft <= 0));
            //context.InProgressResearches.RemoveRange(context.InProgressResearches.Where(r => r.TimeLeft <= 0));

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}