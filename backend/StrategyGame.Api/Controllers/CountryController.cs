using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.Services.Country;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CountryInfo>> GetCurrentStateAsync()
        {
            return Ok(await _countryService.GetCountryInfoAsync(User.Identity.Name));
        }
    }
}