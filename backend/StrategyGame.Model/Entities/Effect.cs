using StrategyGame.Model.Entities.Creations;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents an effect within the UnderSea database.
    /// </summary>
    public class Effect : AbstractEntity<Effect>
    {
        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter for the event.
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets if the effect should only be applied once, when the related event / creation happens / finishes.
        /// </summary>
        public bool IsOneTime { get; set; }

        /// <summary>
        /// Gets or sets the collection of buildings that are using this effect.
        /// </summary>
        public ICollection<Connector<BuildingType, Effect>> AffectedBuildings { get; set; }

        /// <summary>
        /// Gets or sets the collection of researches that are using this effect.
        /// </summary>
        public ICollection<Connector<ResearchType, Effect>> AffectedResearches { get; set; }

        /// <summary>
        /// Gets or sets the collection of events that are using this effect.
        /// </summary>
        public ICollection<Connector<RandomEvent, Effect>> AffectedEvents { get; set; }
    }
}