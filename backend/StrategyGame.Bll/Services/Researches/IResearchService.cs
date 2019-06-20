using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;

namespace StrategyGame.Bll.Services.Researches
{
    /// <summary>
    /// Service for managing the researches of a player.
    /// </summary>
    public interface IResearchService
    {
        /// <summary>
        /// Gets the researches of the given player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <returns>An IEnumerable containing the researches of the player</returns>
         Task<IEnumerable<CreationInfo>> GetResearchesAsync(string username);

        /// <summary>
        /// Starts the desired research for the given player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <param name="researchId">The identifier of the research</param>
        /// <exception cref="InvalidOperationException">Thrown when the player does not have enough money</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the technology does not exist</exception>
        /// <exception cref="InProgressException">Thrown when there is another research in progress</exception>
        /// <exception cref="LimitReachedException">Thrown when the research is already finished</exception>
        /// <returns>A task that can be used to await the asynchronous operation</returns>
         Task StartResearchAsync(string username, int researchId);
    }
}