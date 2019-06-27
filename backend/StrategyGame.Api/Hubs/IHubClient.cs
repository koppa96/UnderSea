using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.Hubs
{
    /// <summary>
    /// Interface for defining methods that can be called on the clients of the strongly typed Hub.
    /// </summary>
    public interface IHubClient
    {
        Task NotifyTurnEnded();
        Task TransferReceived(TransferDetails details);
    }
}
