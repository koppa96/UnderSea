using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Logger
{
    /// <summary>
    /// A service that logs into the database.
    /// </summary>
    public interface IDbLogger
    {
        /// <summary>
        /// Logs the provided exception.
        /// </summary>
        /// <param name="e">The exception to log.</param>
        /// <returns>The task representing the state of the operation.</returns>
        Task LogExceptionAsync(Exception e);

        /// <summary>
        /// Logs the provided request.
        /// </summary>
        /// <returns>The task representing the state of the operation.</returns>
        Task LogRequestAsync(HttpContext context);
    }
}