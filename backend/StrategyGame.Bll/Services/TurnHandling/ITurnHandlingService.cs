using StrategyGame.Dal;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.TurnHandling
{
    /// <summary>
    /// Provides methods to handle end-of-turn calculations.
    /// </summary>
    public interface ITurnHandlingService
    {
        /// <summary>
        /// Updates the data in the <paramref name="context"/> at the end of a turn, calculating buildings, researches, combat, score, etc.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> used to </param>
        /// <param name="cancel">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>The <see cref="Task"/> representing the state of the operation.</returns>
        Task EndTurnAsync(UnderSeaDatabaseContext context, CancellationToken cancel = default);
    }
}