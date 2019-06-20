using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;

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
        /// <returns></returns>
        Task CreateAsync(string username, string countryName);

        /// <summary>
        /// Gets the general information about a user's country.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>The information about the country</returns>
        Task<CountryInfo> GetCountryInfoAsync(string username);

        /// <summary>
        /// Gets a list of players, their countries' score and rank. 
        /// </summary>
        /// <returns>The list of rank infos</returns>
        Task<IEnumerable<RankInfo>> GetRankedListAsync();
    }
}
