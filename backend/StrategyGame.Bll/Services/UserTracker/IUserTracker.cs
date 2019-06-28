using System.Collections.Generic;

namespace StrategyGame.Bll.Services.UserTracker
{
    /// <summary>
    /// Service endpoint to track currently connected users.
    /// </summary>
    public interface IUserTracker
    {
        /// <summary>
        /// Gets the list of usernames that are currently connected.
        /// </summary>
        /// <returns>The list of usernames.</returns>
        IEnumerable<string> GetConnectedUsers();

        /// <summary>
        /// Adds a user to the connected pool.
        /// </summary>
        /// <param name="username">The user to add.</param>
        void AddUser(string username);

        /// <summary>
        /// Removes a user from the connected pool
        /// </summary>
        /// <param name="username">The user to remove.</param>
        void RemoveUser(string username);
    }
}