using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.DTO.Sent;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResearchesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CreationInfo>>> GetResearchesAsync()
        {
            return Ok();
        }

        [HttpPost("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartResearchAsync(int id)
        {
            return Ok();
        }
    }
}