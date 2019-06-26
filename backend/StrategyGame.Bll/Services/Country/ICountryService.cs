using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Country
{
    /// <summary>
    /// A service for endpoints to manipulate countries.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Creates a country for the given user with the desired name.
        /// </summary>
        /// <param name="username">The name of the owner</param>
        /// <param name="countryName">The name of the country</param>
        /// <param name="turnEndWaitToken">The token that can be used to cancel waiting for an in-progress end-of-turn calculation.</param>
        /// <returns>The task representing the operation.</returns>
        /// <exception cref="TaskCanceledException">Thrown if the operation was cancelled.</exception>
        Task CreateAsync(string username, string countryName, CancellationToken turnEndWaitToken = default);

        /// <summary>
        /// Gets the general information about a user's country.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="countryId">The ID of the country to get the information on.</param>
        /// <returns>The information about the country</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="countryId"/> is invalid</exception>
        Task<CountryInfo> GetCountryInfoAsync(string username, int countryId);

        /// <summary>
        /// Gets the general information about all countries a user has.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>The information about the countries of the user.</returns>
        Task<IEnumerable<CountryInfo>> GetCountryInfoAsync(string username);

        /// <summary>
        /// Gets a list of players, their countries' score and rank. 
        /// </summary>
        /// <returns>The list of rank infos</returns>
        Task<IEnumerable<RankInfo>> GetRankedListAsync();
    }
}