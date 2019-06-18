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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UnderSeaDatabase _context;
        private readonly UserManager<User> _userManager;

        public AccountController(UnderSeaDatabase context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var users = await _context.Users.Select(u => u.UserName).ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsnyc([FromBody] RegisterData data)
        {
            var user = new User()
            {
                UserName = data.Username,
                Email = data.Email
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}