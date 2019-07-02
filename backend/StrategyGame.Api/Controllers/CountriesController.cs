using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StrategyGame.Api.Hubs;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.Services.Country;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API endpoint to manage countries.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IHubContext<UnderSeaHub> _hubContext;

        public CountriesController(ICountryService countryService, IHubContext<UnderSeaHub> hubContext)
        {
            _countryService = countryService;
            _hubContext = hubContext;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<BriefCountryInfo>>> GetCountriesAsync()
        {
            return Ok(await _countryService.GetCountriesAsync(User.Identity.Name));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CountryInfo>> GetCurrentStateAsync(int id)
        {
            return Ok(await _countryService.GetCountryInfoAsync(User.Identity.Name, id));
        }

        [HttpPost("send-resources")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SendResourcesAsync([FromBody] TransferDetails details)
        {
            var targetUser = await _countryService.TransferAsync(User.Identity.Name, details);
            await _hubContext.Clients.User(targetUser).SendAsync(nameof(IHubClient.TransferReceived), details);
            return Ok();
        }
    }
}