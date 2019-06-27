using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Services.Buildings;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API endpoint to manage buildings.
    /// </summary>
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
            return Ok(await _buildingService.GetBuildingsAsync());
        }

        [HttpPost("{countryId}/{buildingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartBuildingAsync(int countryId, int buildingId)
        {
            using (var src = new CancellationTokenSource(Constants.DefaultTurnEndTimeout))
            {
                await _buildingService.StartBuildingAsync(User.Identity.Name, countryId, buildingId, src.Token);
                return Ok();
            }
        }
    }
}