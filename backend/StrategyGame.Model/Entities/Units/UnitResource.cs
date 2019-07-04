using StrategyGame.Model.Entities.Connectors;

namespace StrategyGame.Model.Entities.Units
{
    /// <summary>
    /// Linking table between a building and a resource.
    /// </summary>
    public class UnitResource : ConnectorWithAmount<UnitType, ResourceType>
    {
        /// <summary>
        /// Gets or sets the amount the unit costs in maintenance.
        /// </summary>
        public int MaintenanceAmount { get; set; }
    }
}