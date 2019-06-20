using StrategyGame.Model.Entities.Frontend;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a building type within the UnderSea database.
    /// </summary>
    public class BuildingType : AbstractEntity<BuildingType>
    {
        /// <summary>
        /// Gets or sets the amount of pearls the building costs.
        /// </summary>
        public int CostPearl { get; set; }

        /// <summary>
        /// Gets or sets the amount of corals the building costs.
        /// </summary>
        public int CostCoral { get; set; }

        /// <summary>
        /// Gets or sets the built time of the building (in turns).
        /// </summary>
        public int BuildTime { get; set; }

        /// <summary>
        /// Gets or sets the maximal count of a building within a single country. 0 means the building can't be built, negative means it may be built unlimited times.
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BuildingContent"/> of the building.
        /// </summary>
        public virtual BuildingContent Content { get; set; }

        /// <summary>
        /// Gets the collection of effects this building provides.
        /// </summary>
        public virtual ICollection<BuildingEffect> Effects { get; set; } = new HashSet<BuildingEffect>();

        /// <summary>
        /// Gets the collection of buildings of this type that are completed.
        /// </summary>
        public virtual ICollection<CountryBuilding> CompletedBuildings { get; set; } = new HashSet<CountryBuilding>();

        /// <summary>
        /// Gets the collection of buildings of this type that are being built.
        /// </summary>
        public virtual ICollection<InProgressBuilding> InProgressBuildings { get; set; } = new HashSet<InProgressBuilding>();
    }
}