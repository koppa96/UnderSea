using System;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    /// <summary>
    /// A service for endpoints to log exceptions.
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// Logs the provided exception.
        /// </summary>
        /// <param name="e">The exception to log.</param>
        /// <returns>The task representing the state of the operation.</returns>
        Task LogExceptionAsync(Exception e);
    }
}