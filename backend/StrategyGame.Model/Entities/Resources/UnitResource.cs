using StrategyGame.Model.Entities.Units;

namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Linking table between a building and a resource.
    /// </summary>
    public class UnitResource : AbstractConnectorWithAmount<UnitType, ResourceType>
    {
        /// <summary>
        /// Gets or sets the amount the unit costs in maintenance.
        /// </summary>
        public int MaintenanceAmount { get; set; }
    }
}