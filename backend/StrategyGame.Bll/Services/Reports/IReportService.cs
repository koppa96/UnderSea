using StrategyGame.Bll.Dto.Sent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent.Country;

namespace StrategyGame.Bll.Services.Reports
{
    /// <summary>
    /// Service for managing the combat reports of a player.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Lists the command reports 
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="countryId">The id of the country</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the country id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country is not the user's country</exception>
        /// <returns>A sequence containing the reports</returns>
        Task<IEnumerable<CombatInfo>> GetCombatInfoAsync(string username, int countryId);

        /// <summary>
        /// Marks a report as seen for the user.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The identifier of the report</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user has no right to mark the report as seen</exception>
        /// <returns>A task that can be awaited</returns>
        Task SetCombatReportSeenAsync(string username, int reportId);

        /// <summary>
        /// Marks a report as deleted for the user, and removes it from the database if both players marked it as deleted.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The identifier of the report</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user has no right to delete the report</exception>
        /// <returns>A task that can be awaited</returns>
        Task DeleteCombatReportAsync(string username, int reportId);

        /// <summary>
        /// Lists the event reports of the user that happened to the specified country.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="countryId">The id of the country</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the country id is not valid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the country is not the user's country</exception>
        /// <returns>A sequence containing the EventInfos</returns>
        Task<IEnumerable<EventInfo>> GetEventInfoAsync(string username, int countryId);

        /// <summary>
        /// Marks an event report as seen.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The id of the report</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the report is not related to the user's country</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <returns></returns>
        Task SetEventReportSeenAsync(string username, int reportId);

        /// <summary>
        /// Deletes an event report.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="reportId">The id of the report</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user has no right to delete the report</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the report id is not valid</exception>
        /// <returns></returns>
        Task DeleteEventReportAsync(string username, int reportId);
    }
}