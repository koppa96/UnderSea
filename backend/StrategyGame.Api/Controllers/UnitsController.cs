using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.Services.Units;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UnitInfo>>> GetAllUnitsAsync()
        {
            return Ok(await _unitService.GetUnitInfoAsync());
        }

        [HttpPost("{countryid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<IEnumerable<BriefUnitInfo>>> CreateAsync(int countryid, [FromBody] IEnumerable<PurchaseDetails> purchases)
        {
            using (var src = new CancellationTokenSource(Constants.DefaultTurnEndTimeout))
            {
                return Ok(await _unitService.CreateUnitAsync(User.Identity.Name, countryid, purchases, src.Token));
            }
        }

        [HttpDelete("{countryid}/{id}/{count}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int countryid, int id, int count)
        {
            using (var src = new CancellationTokenSource(Constants.DefaultTurnEndTimeout))
            {
                await _unitService.DeleteUnitsAsync(User.Identity.Name, countryid, id, count, src.Token);
                return NoContent();
            }
        }
    }
}