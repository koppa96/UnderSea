using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using System;
using System.Collections.Generic;
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
        /// <returns>The task representing the operation.</returns>
        Task CreateAsync(string username, string countryName);

        /// <summary>
        /// Buys a country for the given user with the desired name.
        /// </summary>
        /// <param name="username">The name of the owner</param>
        /// <param name="countryId">The ID of the country to buy from.</param>
        /// <param name="countryName">The name of the country</param>
        /// <returns>The task representing the operation.</returns>
        Task<BriefCountryInfo> BuyAsync(string username, int countryId, string countryName);

        /// <summary>
        /// Gets a short info about all of the countries of the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>The list of countries</returns>
        Task<IEnumerable<BriefCountryInfo>> GetCountriesAsync(string username);

        /// <summary>
        /// Gets the general information about a user's country.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="countryId">The id of the country</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the country id is invalid.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country id is not the user's country.</exception>
        /// <returns>The information about the country</returns>
        Task<CountryInfo> GetCountryInfoAsync(string username, int countryId);

        /// <summary>
        /// Gets the detailed country information about all of the countries of the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>The detailed infos</returns>
        Task<IEnumerable<CountryInfo>> GetAllCountryInfoAsync(string username);

        /// <summary>
        /// Gets a list of players, their countries' score and rank. 
        /// </summary>
        /// <returns>The list of rank infos</returns>
        Task<IEnumerable<RankInfo>> GetRankedListAsync();

        /// <summary>
        /// Transfers resources from one country to another one.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="details">The details of the transaction</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either country ids are invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when there are not enough resources</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the sender is not the user's country</exception>
        /// <returns>The name of the target user to be notified</returns>
        Task<string> TransferAsync(string username, TransferDetails details);
    }
}