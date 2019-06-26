using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Units;
using System;
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
            try
            {
                return Ok(await _unitService.GetUnitInfoAsync(User.Identity.Name, countryid));
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = e.Message
                });
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
        }

        [HttpPost("{countryid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<IEnumerable<UnitInfo>>> CreateAsync(int countryid, [FromBody] IEnumerable<PurchaseDetails> purchases)
        {
            return Ok(await _unitService.CreateUnitAsync(User.Identity.Name, purchases));
        }

        [HttpDelete("{countryid}/{id}/{count}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int countryid, int id, int count)
        {
            await _unitService.DeleteUnitsAsync(User.Identity.Name, id, count);
            return NoContent();
        }
    }
}