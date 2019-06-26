using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.Services.Country;
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

        [HttpGet("{id}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CountryInfo>> GetCurrentStateAsync(int id)
        {
            try
            {
                return Ok(await _countryService.GetCountryInfoAsync(User.Identity.Name, id));
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = e.Message
                });
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = 401,
                    Title = ErrorMessages.Unauthorized,
                    Detail = e.Message
                });
            }
        }
    }
}