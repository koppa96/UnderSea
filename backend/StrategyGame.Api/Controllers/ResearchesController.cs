using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Researches;

namespace StrategyGame.Api.Controllers
{
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
            return Ok(await _researchService.GetResearchesAsync(User.Identity.Name));
        }

        [HttpPost("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartResearchAsync(int id)
        {
            try
            {
                await _researchService.StartResearchAsync(User.Identity.Name, id);
                return Ok();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = ErrorMessages.NotFound,
                    Detail = ErrorMessages.NoSuchResearch
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
            catch (LimitReachedException)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = ErrorMessages.LimitReached
                });
            }
        }
    }
}