using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Frontend;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Represents a resource in the undersea database.
    /// </summary>
    public class ResourceType : AbstractEntity<ResourceType>
    {
        /// <summary>
        /// Gets or sets the content for the resource.
        /// </summary>
        public ResourceContent Content { get; set; }

        /// <summary>
        /// Gets or sets the starting amount of the resource.
        /// </summary>
        public long StartingAmount { get; set; }

        /// <summary>
        /// Gets or sets the collection of resources countries have.
        /// </summary>
        public ICollection<ConnectorWithAmount<Country, ResourceType>> CountryResources { get; set; }

        /// <summary>
        /// Gets or sets the collection of costs for buildings in the resource.
        /// </summary>
        public ICollection<ConnectorWithAmount<BuildingType, ResourceType>> BuildingResources { get; set; }

        /// <summary>
        /// Gets or sets the collection of costs for researches in the resource.
        /// </summary>
        public ICollection<ConnectorWithAmount<ResearchType, ResourceType>> ResearchResources { get; set; }

        /// <summary>
        /// Gets or sets the collection of costs for units in the resource.
        /// </summary>
        public ICollection<UnitResource> UnitResources { get; set; }

        public ICollection<ReportResource> ReportResources { get; set; }
    }
}