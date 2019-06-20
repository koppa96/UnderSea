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
    public class BuildingsController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CreationInfo>>> GetBuildingsAsync()
        {
            return Ok();
        }

        [HttpPost("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> StartBuildingAsync(int id)
        {
            return Ok();
        }
    }
}