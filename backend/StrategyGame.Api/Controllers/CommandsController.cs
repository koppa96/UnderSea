using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Services.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandService _commandService;

        public CommandsController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CommandInfo>>> GetCommandsAsync(int id)
        {
            return Ok(await _commandService.GetCommandsAsync(User.Identity.Name, id));
        }

        [HttpPost("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CommandInfo>> AttackTargetAsync(int id, [FromBody] CommandDetails command)
        {
            return Ok(await _commandService.AttackTargetAsync(User.Identity.Name, command));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _commandService.DeleteCommandAsync(User.Identity.Name, id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CommandInfo>> UpdateCommandAsync(int id, [FromBody] CommandDetails command)
        {
            return Ok(await _commandService.UpdateCommandAsync(User.Identity.Name, id, command));
        }
    }
}