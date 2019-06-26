using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a user in the UnderSea database.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Get or sets the country that is ruled by the user.
        /// </summary>
        public ICollection<Country> RuledCountries { get; set; }

        /// <summary>
        /// The relative url of the profile image.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}