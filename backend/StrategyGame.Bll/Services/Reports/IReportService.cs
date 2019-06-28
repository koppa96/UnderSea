using StrategyGame.Bll.Dto.Sent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Reports
{
    /// <summary>
    /// Service for managing the combat reports of a player.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Lists all the reports of a user personalized to them.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>A sequence containing the reports</returns>
        Task<IEnumerable<CombatInfo>> GetCombatInfoAsync(string username);

        /// <summary>
        /// Marks a report as seen for the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The identifier of the report</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user has no right to mark the report as seen</exception>
        /// <returns>A task that can be awaited</returns>
        Task SetSeenAsync(string username, int reportId);

        /// <summary>
        /// Marks a report as deleted for the user, and removes it from the database if both players marked it as deleted.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The identifier of the report</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user has no right to delete the report</exception>
        /// <returns>A task that can be awaited</returns>
        Task DeleteAsync(string username, int reportId);
    }
}