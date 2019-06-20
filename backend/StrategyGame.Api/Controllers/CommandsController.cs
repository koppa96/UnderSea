using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Api.ControllerExtensions;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.DTO.Sent;
using StrategyGame.Bll.Services.Commands;

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

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CommandInfo>>> GetCommandsAsync()
        {
            return Ok(await _commandService.GetCommandsAsync(User.Identity.Name));
        }

        [HttpPost]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CommandInfo>> AttackTargetAsync([FromBody] CommandDetails command)
        {
            try
            {
                return Ok(await _commandService.AttackTargetAsync(User.Identity.Name, command));
            }
            catch (ArgumentOutOfRangeException e)
            {
                return this.HandleNotFound(command, e);
            } catch (ArgumentException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = "You don't have enough units to perform this command."
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _commandService.DeleteCommandAsync(User.Identity.Name, id);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = "Command not found."
                });
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CommandInfo>> UpdateCommand(int id, [FromBody] CommandDetails command)
        {
            try
            {
                return Ok(_commandService.UpdateCommandAsync(User.Identity.Name, id, command));
            }
            catch (ArgumentOutOfRangeException e)
            {
                return this.HandleNotFound(command, e);
            }
            catch (ArgumentException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = "You don't have enough units to perform this command."
                });
            }
        }
    }
}