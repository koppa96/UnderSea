using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.DTO
{
    public class RegisterData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
    }
}
