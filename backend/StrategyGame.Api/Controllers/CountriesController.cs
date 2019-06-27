﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.Services.Country;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StrategyGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<BriefCountryInfo>>> GetCountriesAsync()
        {
            return Ok(await _countryService.GetCountriesAsync(User.Identity.Name));
        }

        [HttpPut("{id}/{name}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<BriefCountryInfo>> BuyCountryAsync(int id, string name)
        {
            return Ok(await _countryService.BuyAsync(User.Identity.Name, id, name));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CountryInfo>> GetCurrentStateAsync(int id)
        {
            return Ok(await _countryService.GetCountryInfoAsync(User.Identity.Name, id));
        }
    }
}