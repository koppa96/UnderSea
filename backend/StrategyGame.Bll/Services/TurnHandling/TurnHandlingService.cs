using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using System;
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
            var globals = await context.GlobalValues.SingleAsync(cancel);

            var preCombat = context.Countries
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(d => d.Unit)
                .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                            .ThenInclude(b => b.Effect)
                .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(r => r.Effect)
                .Include(c => c.InProgressBuildings)
                    .ThenInclude(b => b.Building)
                .Include(c => c.InProgressResearches)
                    .ThenInclude(r => r.Research)
                .Include(c => c.CurrentEvent)
                    .ThenInclude(e => e.Effects)
                            .ThenInclude(e => e.Effect);

            var events = await context.RandomEvents
                .Include(e => e.Effects)
                    .ThenInclude(e => e.Effect)
                .ToListAsync();

            await preCombat.ForEachAsync(c => Handler.HandlePreCombat(context, c, globals, events));

            var combat = context.Countries
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(d => d.Unit)
                .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                            .ThenInclude(b => b.Effect)
                .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(r => r.Effect)
                .Include(c => c.IncomingAttacks)
                    .ThenInclude(a => a.Divisions)
                        .ThenInclude(d => d.Unit)
                .Include(c => c.IncomingAttacks)
                    .ThenInclude(a => a.ParentCountry)
                        .ThenInclude(pc => pc.Buildings)
                            .ThenInclude(pb => pb.Building)
                                .ThenInclude(pb => pb.Effects)
                                    .ThenInclude(pb => pb.Effect)
                .Include(c => c.IncomingAttacks)
                    .ThenInclude(a => a.ParentCountry)
                        .ThenInclude(pc => pc.Researches)
                            .ThenInclude(pr => pr.Research)
                                .ThenInclude(pr => pr.Effects)
                                    .ThenInclude(pr => pr.Effect)
                .Include(c => c.CurrentEvent)
                    .ThenInclude(e => e.Effects)
                            .ThenInclude(e => e.Effect);

            await combat.ForEachAsync(c => Handler.HandleCombat(context, c, globals));

            var postCombat = context.Countries
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                            .ThenInclude(b => b.Effect)
                .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(r => r.Effect)
                .Include(c => c.CurrentEvent)
                    .ThenInclude(e => e.Effects)
                        .ThenInclude(e => e.Effect);

            await postCombat.ForEachAsync(c => Handler.HandlePostCombat(context, c, globals));

            int index = 0;
            await context.Countries.OrderByDescending(c => c.Score).ForEachAsync(c => c.Rank = ++index);

            globals.Round++;

            await context.SaveChangesAsync();

            //TODO: investigate why EF produces null bdgs in the below code (this is why there is a save above)
            //var first = context.CountryBuildings.First();
            //first.Count = 10;
            ////context.CountryBuildings.First().Count = 10;
            //var count = context.CountryBuildings.First().Count;
            //var bdgs = context.CountryBuildings.FirstOrDefault(x => x.Count == 10);

            context.InProgressBuildings.RemoveRange(context.InProgressBuildings.Where(b => b.TimeLeft <= 0));
            context.InProgressResearches.RemoveRange(context.InProgressResearches.Where(r => r.TimeLeft <= 0));

            await context.SaveChangesAsync();
        }
    }
}