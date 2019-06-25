using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Received.UserManagement;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.UserManagement;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Model.Entities;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// API Endpoint for creating accounts and querying the currently logged in user's data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ICountryService _countryService;

        public AccountsController(UserManager<User> userManager, ICountryService countryService)
        {
            _userManager = userManager;
            _countryService = countryService;
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserInfo>> GetAccountAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(new UserInfo
            {
                Username = currentUser.UserName,
                Email = currentUser.Email,
                ProfileImageUrl = currentUser.ImageUrl
            });
        }

        [HttpPut]
        [Authorize]
        [Route("me/image")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> SaveProvileImageAsync()
        {
            var filename = User.Identity.Name + "." + Request.Headers["Content-Type"].First().Split("/")[1].Split("+")[0];
            var path = Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\images\profile\" + filename);
            
            using (var fileStream = System.IO.File.OpenWrite(path))
            {
                await Request.Body.CopyToAsync(fileStream);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.ImageUrl = $"images/profile/{filename}";
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("me/password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] PasswordChangeData data)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _userManager.ChangePasswordAsync(user, data.OldPassword, data.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.First().Code == "PasswordMismatch")
                {
                    return Unauthorized(new ProblemDetails
                    {
                        Status = 401,
                        Title = ErrorMessages.Unauthorized,
                        Detail = "Invalid current password."
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = ErrorMessages.BadRequest,
                    Detail = "Invalid new password. The password must contain at least 6 characters, and at least one uppercase, one lowercase and on number."
                });
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<string>>> GetUsernamesAsync()
        {
            return Ok(await _userManager.Users.Select(u => u.UserName).ToListAsync());
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CreateAccountAsnyc([FromBody] RegisterData data)
        {
            if (await _userManager.FindByNameAsync(data.Username) != null)
            {
                return BadRequest("Duplicate username");
            }

            var user = new User()
            {
                UserName = data.Username,
                Email = data.Email,
                ImageUrl = "images/static/defaultprofile.svg"
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _countryService.CreateAsync(data.Username, data.CountryName);
            return StatusCode(201, new UserInfo
            {
                Username = user.UserName,
                Email = user.Email
            });
        }

        [HttpGet]
        [Authorize]
        [Route("ranked")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<RankInfo>>> GetRankedListAsync()
        {
            return Ok(await _countryService.GetRankedListAsync());
        }
    }
}