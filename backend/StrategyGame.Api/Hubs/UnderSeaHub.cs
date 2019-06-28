using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StrategyGame.Bll.Services.UserTracker;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Api.Hubs
{
    [Authorize]
    public class UnderSeaHub : Hub<IHubClient>
    {
        private readonly IUserTracker _userTracker;

        public UnderSeaHub(IUserTracker userTracker)
        {
            _userTracker = userTracker;
        }

        public override Task OnConnectedAsync()
        {
            _userTracker.AddUser(Context.UserIdentifier);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userTracker.RemoveUser(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }
    }
}