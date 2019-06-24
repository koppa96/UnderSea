using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Buildings;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CreationInfo>>> GetBuildingsAsync()
        {
            return Ok(await _buildingService.GetBuildingsAsync(User.Identity.Name));
        }

        [HttpPost("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartBuildingAsync(int id)
        {
            try
            {
                await _buildingService.StartBuildingAsync(User.Identity.Name, id);
                return Ok();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = ErrorMessages.NotFound,
                    Detail = ErrorMessages.NoSuchBuilding
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = ErrorMessages.NotEnoughMoney
                });
            }
            catch (InProgressException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = ErrorMessages.InProgress
                });
            }
        }
    }
}