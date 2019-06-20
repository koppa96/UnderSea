using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.DTO.Sent;
using StrategyGame.Bll.Services.Units;

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
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UnitInfo>>> GetAllUnitsAsync()
        {
            return Ok(await _unitService.GetUnitInfoAsync(User.Identity.Name));
        }

        [HttpPost("{id}/{count}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<UnitInfo>> CreateAsync(int id, int count)
        {
            try
            {
                return Ok(await _unitService.CreateUnitAsync(User.Identity.Name, id, count));
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = "No unit type found with the given ID."
                });
            }
            catch (ArgumentException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = "Invalid amount. The amount must be a positive number and you have to have enough money."
                });
            }
        }

        [HttpDelete("{id}/{count}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAsync(int id, int count)
        {
            try
            {
                await _unitService.DeleteUnitsAsync(User.Identity.Name, id, count);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = "No unit type found with the given ID."
                });
            }
            catch (ArgumentException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = "Invalid amount. The amount must be a positive number and not more than the amount of units you have."
                });
            }
        }
    }
}