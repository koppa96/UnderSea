using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Buildings
{
    /// <summary>
    /// Service for managing the buildings of the player.
    /// </summary>
    public interface IBuildingService
    {
        /// <summary>
        /// Gets the buildings for the player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <param name="countryId">The id of the country</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country id is not the user's country id</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the country id is invalid</exception>
        /// <returns>The information about the buildings</returns>
         Task<IEnumerable<CreationInfo>> GetBuildingsAsync(string username, int countryId);

        /// <summary>
        /// Starts the building of a building for the player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <param name="buildingId">The identifier of the building type</param>
        /// <param name="countryId">The id of the country</param>
        /// <exception cref="InvalidOperationException">Thrown when the player does not have enough money</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the id of the building is invalid</exception>
        /// <exception cref="InProgressException">Thrown when when a building is already in progress</exception>
        /// <returns>A task that can be awaited</returns>
         Task StartBuildingAsync(string username, int countryId, int buildingId);
    }
}