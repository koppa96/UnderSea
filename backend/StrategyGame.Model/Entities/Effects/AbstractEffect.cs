using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Effects
{
    /// <summary>
    /// Represents an effect within the UnderSea database.
    /// </summary>
    public abstract class AbstractEffect : AbstractEntity<AbstractEffect>
    {
        /// <summary>
        /// Gets or sets the value of the effect.
        /// </summary>
        public double Value { get; set; }

        public ICollection<BuildingEffect> AffectedBuildings { get; set; }
        public ICollection<ResearchEffect> AffectedResearches { get; set; }

        /// <summary>
        /// Applies the effect to the target country. Beware of saving!
        /// </summary>
        /// <param name="target">The country to apply to.</param>
        public abstract void Apply(Country target);
    }
}