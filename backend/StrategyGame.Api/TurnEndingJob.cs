using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Api
{
    /// <summary>
    /// Provides a job that can be executed to handle the turn's end.
    /// </summary>
    public class TurnEndingJob
    {
        protected UnderSeaDatabaseContext Context { get; }

        protected ITurnHandlingService Handler { get; }
        
        public TurnEndingJob(UnderSeaDatabaseContext context, ITurnHandlingService handler)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public Task EndTurnASync()
        {
            return Handler.EndTurnAsync(Context);
        }
    }
}