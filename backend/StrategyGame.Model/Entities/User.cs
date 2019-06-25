using Microsoft.AspNetCore.Identity;

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
        public Country RuledCountry { get; set; }

        /// <summary>
        /// The Id of the ruled country.
        /// </summary>
        public int? RuledCountryId { get; set; }

        /// <summary>
        /// The relative url of the profile image.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}