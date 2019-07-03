using StrategyGame.Model.Entities.Effects;
using StrategyGame.Model.Entities.Frontend;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a random event that can happen at the end of turns in the UnderSea database.
    /// </summary>
    public class RandomEvent : AbstractEntity<RandomEvent>
    {
        /// <summary>
        /// Gets or sets the content of the event.
        /// </summary>
        public virtual EventContent Content { get; set; }

        /// <summary>
        /// Gets or sets the collection of effects this event has.
        /// </summary>
        public virtual ICollection<EventEffect> Effects { get; set; }

        /// <summary>
        /// Gets or sets the collection of countries that have this effect currently.
        /// </summary>
        public virtual ICollection<Country> ParentCountries { get; set; }
    }
}