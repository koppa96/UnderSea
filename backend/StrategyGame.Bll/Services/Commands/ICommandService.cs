﻿using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Commands
{
    /// <summary>
    /// Service for interacting with the commands of a player.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Gets the commands that will be executed in the end of the round for the player.
        /// </summary>
        /// <param name="username">The name of the player</param>
        /// <returns>An IEnumerable containing the commands</returns>
        Task<IEnumerable<CommandInfo>> GetCommandsAsync(string username);

        /// <summary>
        /// Creates a new attack command for the user with the given details.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="details">The details of the command</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the target country is not found or a unit type is not found</exception>
        /// <exception cref="ArgumentException">Thrown when there are not enough units to start this command</exception>
        /// <returns>The CommandInfo representing the command</returns>
        Task<CommandInfo> AttackTargetAsync(string username, CommandDetails details);

        /// <summary>
        /// Deletes the command with the given id.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="commandId">The identifier of the command</param>
        /// <exception cref="InvalidOperationException">Thrown when the user is not authorized to delete that command</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the command does not exist</exception>
        /// <returns>A task that can be awaited</returns>
        Task DeleteCommandAsync(string username, int commandId);

        /// <summary>
        /// Changes an existing command to the given details.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <param name="commandId">The identifier of the command</param>
        /// <param name="details">The new details</param>
        /// <exception cref="InvalidOperationException">Thrown when the user is not authorized to change the command</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the target country is not found or a unit type is not found</exception>
        /// <exception cref="ArgumentException">Thrown when there are not enough units to start this command</exception>
        /// <returns></returns>
        Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details);
    }
}
