using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Services.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyGame.Bll.Dto.Sent.Country;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API endpoint to manage reports.
    /// </summary>
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

        [HttpGet("combat/{countryId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CombatInfo>>> GetBattleInfoAsync(int countryId)
        {
            return Ok(await _reportService.GetCombatInfoAsync(User.Identity.Name, countryId));
        }

        [HttpPost("combat/seen/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SetSeenAsync(int id)
        {
            await _reportService.SetCombatReportSeenAsync(User.Identity.Name, id);
            return Ok();
        }

        [HttpDelete("combat/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReportAsync(int id)
        {
            await _reportService.DeleteCombatReportAsync(User.Identity.Name, id);
            return NoContent();
        }

        [HttpGet("event/{countryId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<EventInfo>>> GetEventInfoAsync(int countryId)
        {
            return Ok(await _reportService.GetEventInfoAsync(User.Identity.Name, countryId));
        }

        [HttpPost("event/seen/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SetEventSeenAsync(int id)
        {
            await _reportService.SetEventReportSeenAsync(User.Identity.Name, id);
            return Ok();
        }

        [HttpDelete("event/{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteEventReportAsync(int id)
        {
            await _reportService.DeleteEventReportAsync(User.Identity.Name, id);
            return NoContent();
        }
    }
}
