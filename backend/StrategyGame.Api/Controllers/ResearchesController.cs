using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Services.Researches;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API endpoint to manage researches.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResearchesController : ControllerBase
    {
        private readonly IResearchService _researchService;

        public ResearchesController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CreationInfo>>> GetResearchesAsync()
        {
            return Ok(await _researchService.GetResearchesAsync());
        }

        [HttpPost("{countryId}/{researchId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartResearchAsync(int countryId, int researchId)
        {
            using (var src = new CancellationTokenSource(Constants.DefaultTurnEndTimeout))
            {
                await _researchService.StartResearchAsync(User.Identity.Name, countryId, researchId, src.Token);
                return Ok();
            }
        }
    }
}