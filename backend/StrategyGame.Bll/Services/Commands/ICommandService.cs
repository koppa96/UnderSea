using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.DTO.Sent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Commands
{
    public interface ICommandService
    {
        Task<IEnumerable<CommandInfo>> GetCommandsAsync(string username);
        Task<CommandInfo> AttackTargetAsync(string username, CommandDetails details);
        Task DeleteCommandAsync(string username, int commandId);
        Task<CommandInfo> UpdateCommandAsync(string username, int commandId, CommandDetails details);
    }
}
