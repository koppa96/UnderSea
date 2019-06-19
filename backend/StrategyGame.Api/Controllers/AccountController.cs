using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Api.DTO;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;

namespace StrategyGame.Api.Controllers
{
    /// <summary>
    /// Endpoint for creating, querying and deleting accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserData>> GetAccountsAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(new UserData
            {
                Username = currentUser.UserName,
                Email = currentUser.Email
            });
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
                Email = data.Email
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return StatusCode(201, new UserData
            {
                Username = user.UserName,
                Email = user.Email
            });
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAccountAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return StatusCode(500);
        }
    }
}