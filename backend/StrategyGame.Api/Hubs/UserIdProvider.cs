using Microsoft.AspNetCore.SignalR;

namespace StrategyGame.Api.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}