using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Api.Hubs;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Api
{
    /// <summary>
    /// Provides a job that can be executed to handle the turn's end.
    /// </summary>
    public class TurnEndingJob
    {
        public TurnEndingJob(UnderSeaDatabaseContext context, ITurnHandlingService handler,
            IHubContext<UnderSeaHub> hub, ICountryService service, UserManager<User> users)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Hub = hub ?? throw new ArgumentNullException(nameof(hub));
            Service = service ?? throw new ArgumentNullException(nameof(service));
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }

        protected UnderSeaDatabaseContext Context { get; }

        protected ITurnHandlingService Handler { get; }

        protected IHubContext<UnderSeaHub> Hub { get; }

        protected ICountryService Service { get; }

        protected UserManager<User> Users { get; }

        public async Task EndTurnAsync()
        {
            await Handler.EndTurnAsync(Context);

            await Users.Users.ForEachAsync(async u =>
            {
                var info = await Service.GetCountryInfoAsync(u.UserName);
                await Hub.Clients.User(u.UserName).SendAsync(nameof(IHubClient.ReceiveResultsAsync), info);
            });
        }
    }
}