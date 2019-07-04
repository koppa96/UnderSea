using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Reports;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Creations
{
    /// <summary>
    /// Represents a building type within the UnderSea database.
    /// </summary>
    public class BuildingType : AbstractCreationType<BuildingType, ConnectorWithAmount<BuildingType, ResourceType>, BuildingContent>
    {
        /// <summary>
        /// Gets or sets the built time of the building (in turns).
        /// </summary>
        public int BuildTime { get; set; }

        /// <summary>
        /// Gets or sets the maximal count of a building within a single country.
        /// 0 means the building can't be built, negative means it may be built unlimited times.
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// Gets or sets if the building is a starting building.
        /// </summary>
        public bool IsStarting { get; set; }

        /// <summary>
        /// Gets the collection of effects this building provides.
        /// </summary>
        public virtual ICollection<Connector<BuildingType, Effect>> Effects { get; set; }

        /// <summary>
        /// Gets the collection of buildings of this type that are completed.
        /// </summary>
        public virtual ICollection<ConnectorWithAmount<Country, BuildingType>> CompletedBuildings { get; set; }

        /// <summary>
        /// Gets the collection of buildings of this type that are being built.
        /// </summary>
        public virtual ICollection<ConnectorWithProgress<Country, BuildingType>> InProgressBuildings { get; set; }

        public ICollection<ConnectorWithAmount<CombatReport, BuildingType>> ReportBuildings { get; set; }
    }
}