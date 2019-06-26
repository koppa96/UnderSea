using StrategyGame.Bll.Dto.Sent.Country;
using System.Threading.Tasks;

namespace StrategyGame.Api.Hubs
{
    /// <summary>
    /// Interface for defining methods that can be called on the clients of the strongly typed Hub.
    /// </summary>
    public interface IHubClient
    {
        Task NotifyTurnEnded();
    }
}