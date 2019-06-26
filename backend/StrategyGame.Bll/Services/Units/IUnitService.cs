using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
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
        /// <returns>An IEnumerable containing the UnitInfos</returns>
        Task<IEnumerable<UnitInfo>> GetUnitInfoAsync();

        /// <summary>
        /// Creates the desired amount of units of the given type to the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="purchases">The details of the purchases</param>
        /// <param name="countryId">The ID of the country to get units for.</param>
        /// <param name="turnEndWaitToken">The token that can be used to cancel waiting for an in-progress end-of-turn calculation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the unit id is invalid</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the country is invalid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country id is not the user's country</exception>
        /// <exception cref="ArgumentException">Thrown when the count is not a valid amount</exception>
        /// <exception cref="LimitReachedException">Thrown when the unit would be exceeded by the creation of units</exception>
        /// <exception cref="InvalidOperationException">Thrown when there is not enough money to hire the units</exception>
        /// <exception cref="TaskCanceledException">Thrown if the operation was cancelled.</exception>
        /// <returns>A UnitInfo containing the new amount of units</returns>
        Task<IEnumerable<BriefUnitInfo>> CreateUnitAsync(string username, int countryId,
            IEnumerable<PurchaseDetails> purchases, CancellationToken turnEndWaitToken = default);

        /// <summary>
        /// Deletes the desired amount of units of the given type.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="unitId">The identifier of the unit</param>
        /// <param name="count">The amount of units to be deleted</param>
        /// <param name="countryId">The ID of the country to get units for.</param>
        /// <param name="turnEndWaitToken">The token that can be used to cancel waiting for an in-progress end-of-turn calculation.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the unit id is invalid</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the country is invalid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country id is not the user's country</exception>
        /// <exception cref="ArgumentException">Thrown when the count is not a valid amount</exception>
        /// <exception cref="TaskCanceledException">Thrown if the operation was cancelled.</exception>
        /// <returns>A task that can be awaited</returns>
        Task DeleteUnitsAsync(string username, int countryId, int unitId, int count,
            CancellationToken turnEndWaitToken = default);
    }
}
