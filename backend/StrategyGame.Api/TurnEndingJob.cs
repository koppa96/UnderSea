using Microsoft.AspNetCore.SignalR;
using StrategyGame.Api.Hubs;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Bll.Services.UserTracker;
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

        protected IHubContext<UnderSeaHub> Hub { get; }

        protected ICountryService Service { get; }

        protected IUserTracker Tracker { get; }

        public TurnEndingJob(UnderSeaDatabaseContext context,
            ITurnHandlingService handler, IHubContext<UnderSeaHub> hub,
            ICountryService service, IUserTracker tracker)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Hub = hub ?? throw new ArgumentNullException(nameof(hub));
            Service = service ?? throw new ArgumentNullException(nameof(service));
            Tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        }

        public async Task EndTurnAsync()
        {
            await Handler.EndTurnAsync(Context);

            await Hub.Clients.All.SendAsync(nameof(IHubClient.NotifyTurnEnded));
        }
    }
}