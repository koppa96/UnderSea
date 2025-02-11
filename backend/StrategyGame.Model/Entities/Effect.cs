﻿using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents an effect within the UnderSea database.
    /// </summary>
    public class Effect : AbstractEntity<Effect>
    {
        /// <summary>
        /// Gets or sets the value of the effect.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the target entity of the event.
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Gets or sets the collection of buildings that are using this effect.
        /// </summary>
        public ICollection<BuildingEffect> AffectedBuildings { get; set; }

        /// <summary>
        /// Gets or sets the collection of researches that are using this effect.
        /// </summary>
        public ICollection<ResearchEffect> AffectedResearches { get; set; }

        /// <summary>
        /// Gets or sets the collection of events that are using this effect.
        /// </summary>
        public ICollection<EventEffect> AffectedEvents { get; set; }
    }
}