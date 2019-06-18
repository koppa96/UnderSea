using StrategyGame.Model.Entities.Frontend;
using System;
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
        /// Gets or sets the maximal count of a building within a single country.
        /// </summary>
        /// <remarks>
        /// 0 means the building can't be built, negative means it may be built unlimited times.
        /// </remarks>
        public int MaxCount { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BuildingContent"/> of the building.
        /// </summary>
        public virtual BuildingContent Content { get; set; }

        /// <summary>
        /// Gets the collection of effects this building provides.
        /// </summary>
        public virtual ICollection<BuildingEffect> Effects { get; set; }

        /// <summary>
        /// Gets the collection of buildings of this type that are completed.
        /// </summary>
        public virtual ICollection<CountryBuilding> CompletedBuildings { get; set; }

        /// <summary>
        /// Gets the collection of buildings of this type that are being built.
        /// </summary>
        public virtual ICollection<InProgressBuilding> InProgressBuildings { get; set; }



        /// <summary>
        /// Initializes a new instance of <see cref="BuildingType"/>.
        /// </summary>
        public BuildingType()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingType"/>.
        /// </summary>
        /// <param name="costPearl">The amount of pearls the building costs.</param>
        /// <param name="costCoral">The amount of corals the building costs.</param>
        /// <param name="buildTime">The built time of the building (in turns).</param>
        /// <param name="maxCount">The maximal count of a building within a single country.</param>
        /// <exception cref="ArgumentException">Thrown if the build time was negative.</exception>
        public BuildingType(int costPearl, int costCoral, int buildTime, int maxCount = -1)
        {
            if (buildTime < 0)
            {
                throw new ArgumentException("The research time may not be negative.", nameof(buildTime));
            }

            CostPearl = costPearl;
            CostCoral = costCoral;
            BuildTime = buildTime;
            MaxCount = maxCount;
        }
    }
}