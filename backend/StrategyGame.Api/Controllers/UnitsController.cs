using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.Services.Units;
using System.Collections.Generic;
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

        [HttpGet("{countryid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UnitInfo>>> GetAllUnitsAsync(int countryid)
        {
            return Ok(await _unitService.GetUnitInfoAsync(User.Identity.Name, countryid));
        }

        [HttpPost("{countryid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<IEnumerable<UnitInfo>>> CreateAsync(int countryid, [FromBody] IEnumerable<PurchaseDetails> purchases)
        {
            return Ok(await _unitService.CreateUnitAsync(User.Identity.Name, countryid, purchases));
        }

        [HttpDelete("{countryid}/{id}/{count}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int countryid, int id, int count)
        {
            await _unitService.DeleteUnitsAsync(User.Identity.Name, countryid, id, count);
            return NoContent();
        }
    }
}