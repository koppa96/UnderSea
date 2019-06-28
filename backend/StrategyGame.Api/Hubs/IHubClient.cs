using StrategyGame.Bll.Dto.Sent.Country;
using System.Threading.Tasks;

namespace StrategyGame.Api.Hubs
{
    /// <summary>
    /// Interface for defining methods that can be called on the clients of the strongly typed Hub.
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// The clients receive turn results at the end of the turn.
        /// </summary>
        /// <param name="countryInfo">The updated <see cref="CountryInfo"/>.</param>
        /// <returns>The task representing the operation.</returns>
        Task ReceiveResultsAsync(CountryInfo countryInfo);
    }
}