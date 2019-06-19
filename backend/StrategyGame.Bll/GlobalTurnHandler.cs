using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll
{
    public class GlobalTurnHandler
    {
        public CountryTurnHandler Handler { get; }



        public async Task EndTurnAsync(UnderSeaDatabase context, CancellationToken cancel)
        {
            var turnData = new Dictionary<int, (CountryModifierBuilder Builder, long Coral, long Pearl, long Score)>();

            void HandleLoot(object sender, CountryLootedEventArgs Args)
            {
                var data = turnData[Args.Looter.Id];
                data.Coral += Args.LootedCorals;
                data.Pearl += Args.LootedPearls;
            }

            Handler.CountryLooted += HandleLoot;

            // TODO parallelize?
            foreach (var c in context.Countries)
            {
                var builder = await Handler.HandleTurnBeginingAsync(context, c, cancel).ConfigureAwait(false);
                turnData.Add(c.Id, (builder, 0, 0, 0));
            }

            foreach (var c in context.Countries)
            {
                await Handler.HandleCombatAsync(context, c, turnData[c.Id].Builder, cancel).ConfigureAwait(false);
            }

            // TODO parallelize
            foreach (var c in context.Countries)
            {
                await Handler.CalculateScoreAsync(context, c, turnData[c.Id].Builder, cancel).ConfigureAwait(false);
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            Handler.CountryLooted -= HandleLoot;
        }
    }
}