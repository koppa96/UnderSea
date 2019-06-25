using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Services.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CombatInfo>>> GetBattleInfoAsync()
        {
            return Ok(await _reportService.GetCombatInfoAsync(User.Identity.Name));
        }

        [HttpPost("seen/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SetSeenAsync(int id)
        {
            try
            {
                await _reportService.SetSeenAsync(User.Identity.Name, id);
                return Ok();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = 401,
                    Title = ErrorMessages.Unauthorized,
                    Detail = e.Message
                });
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = ErrorMessages.NotFound,
                    Detail = e.Message
                });
            }
        }

        [HttpDelete("{id]")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReportAsync(int id)
        {
            try
            {
                await _reportService.DeleteAsync(User.Identity.Name, id);
                return NoContent();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = 401,
                    Title = ErrorMessages.Unauthorized,
                    Detail = e.Message
                });
            }
            catch (ArgumentOutOfRangeException e)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = ErrorMessages.NotFound,
                    Detail = e.Message
                });
            }
        }
    }
}
