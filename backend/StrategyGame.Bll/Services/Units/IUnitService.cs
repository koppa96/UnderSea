using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Units
{
    /// <summary>
    /// Service for managing units of a player.
    /// </summary>
    public interface IUnitService
    {
        /// <summary>
        /// Gets UnitInfos for the given player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <returns>An IEnumerable containing the UnitInfos</returns>
        Task<IEnumerable<UnitInfo>> GetUnitInfoAsync(string username);

        /// <summary>
        /// Creates the desired amount of units of the given type to the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="unitId">The identifier of the unit</param>
        /// <param name="count">The amount of units to be created</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the unitId is invalid</exception>
        /// <exception cref="ArgumentException">Thrown when the count is not a valid amount</exception>
        /// <exception cref="LimitReachedException">Thrown when the unit would be exceeded by the creation of units</exception>
        /// <exception cref="InvalidOperationException">Thrown when there is not enough money to hire the units</exception>
        /// <returns>A UnitInfo containing the new amount of units</returns>
        Task<UnitInfo> CreateUnitAsync(string username, int unitId, int count);

        /// <summary>
        /// Deletes the desired amount of units of the given type.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="unitId">The identifier of the unit</param>
        /// <param name="count">The amount of units to be deleted</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the unitId is invalid</exception>
        /// <exception cref="ArgumentException">Thrown when the count is not a valid amount</exception>
        /// <returns>A task that can be awaited</returns>
        Task DeleteUnitsAsync(string username, int unitId, int count);
    }
}
