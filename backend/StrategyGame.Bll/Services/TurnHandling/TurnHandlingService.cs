using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.TurnHandling
{
    public class TurnHandlingService : ITurnHandlingService
    {
        protected CountryTurnHandler Handler { get; }

        protected AsyncReaderWriterLock TurnEndLock { get; }

        public TurnHandlingService(ModifierParserContainer parsers, AsyncReaderWriterLock turnEndLock)
        {
            Handler = new CountryTurnHandler(parsers ?? throw new ArgumentNullException(nameof(parsers)));
            TurnEndLock = turnEndLock ?? throw new ArgumentNullException(nameof(turnEndLock));
        }

        public async Task EndTurnAsync(UnderSeaDatabaseContext context, CancellationToken cancel = default)
        {
            // The below code is capable of producing an error in EF (latest at 2019.06)
            // "bdgs" becomes null, while "bdgs2" is the expectd value. EF appears to not track changes when querying
            // the context directly, only when accessing things through nav properties.
            //var first = context.CountryBuildings.Include(b => b.ParentCountry).First();
            //first.Count = 10;
            //var count = context.CountryBuildings.First().Count;
            //var bdgs = context.CountryBuildings.FirstOrDefault(x => x.Count == 10);
            //var cnt = context.Countries.Include(c => c.Buildings).FirstOrDefault(x => x.Id == first.ParentCountry.Id);
            //var bdgs2 = cnt.Buildings.FirstOrDefault(x => x.Count == 10);

            using (var lck = await TurnEndLock.WriterLockAsync())
            {
                var timer = Stopwatch.StartNew();
                var globals = await context.GlobalValues.SingleAsync(cancel);

                await context.InProgressBuildings.ForEachAsync(b => b.TimeLeft--);
                await context.InProgressResearches.ForEachAsync(r => r.TimeLeft--);

                // In progress stuff's effects have to be included, as those might turn into actual effects
                // and if they do, the combat method's include would not find them, causing a null-reference.
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
                            .ThenInclude(b => b.Effects)
                                .ThenInclude(b => b.Effect)
                    .Include(c => c.InProgressResearches)
                        .ThenInclude(r => r.Research)
                            .ThenInclude(r => r.Effects)
                                .ThenInclude(r => r.Effect)
                    .Include(c => c.CurrentEvent)
                        .ThenInclude(e => e.Effects)
                                .ThenInclude(e => e.Effect);

                var events = await context.RandomEvents
                    .Include(e => e.Effects)
                        .ThenInclude(e => e.Effect)
                    .ToListAsync();

                await preCombat.ForEachAsync(c => Handler.HandlePreCombat(context, c, globals, events));

                timer.Stop();
                var preCombatTime = timer.ElapsedMilliseconds;
                timer.Restart();

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
                                .ThenInclude(e => e.Effect)
                    .Include(c => c.ParentUser);

                await combat.ForEachAsync(c => Handler.HandleCombat(context, c, globals));

                timer.Stop();
                var combatTime = timer.ElapsedMilliseconds;
                timer.Restart();

                var postCombat = context.Countries
                    .Include(c => c.Commands)
                        .ThenInclude(c => c.Divisions)
                    .Include(c => c.Attacks)
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

                long index = 0;
                await context.Countries.OrderByDescending(c => c.Score).ForEachAsync(c => c.Rank = ++index);

                globals.Round++;

                timer.Stop();
                var postCombatTime = timer.ElapsedMilliseconds;
                timer.Restart();

                // See comment block at the start of the method as to why a save is needed here
                await context.SaveChangesAsync();

                timer.Stop();
                var saveTime = timer.ElapsedMilliseconds;
                timer.Restart();

                context.InProgressBuildings.RemoveRange(context.InProgressBuildings.Where(b => b.TimeLeft <= 0));
                context.InProgressResearches.RemoveRange(context.InProgressResearches.Where(r => r.TimeLeft <= 0));

                await context.SaveChangesAsync();

                timer.Stop();
                Debug.WriteLine("Turn completed, total elapsed time: {0} ms", preCombatTime + postCombatTime + combatTime + saveTime + timer.ElapsedMilliseconds);
                Debug.WriteLine("   Pre-combat time: {0} ms", preCombatTime);
                Debug.WriteLine("   Combat time: {0} ms", combatTime);
                Debug.WriteLine("   Post-combat time: {0} ms", postCombatTime);
                Debug.WriteLine("   Save time: {0} ms", saveTime);
                Debug.WriteLine("   In-progress delete time: {0} ms", timer.ElapsedMilliseconds);
            }
        }
    }
}