using StrategyGame.Model.Entities.Connectors;

namespace StrategyGame.Model.Entities.Reports
{
    /// <summary>
    /// Linking table between a report and a resource.
    /// </summary>
    public class ReportResource : ConnectorWithAmount<CombatReport, ResourceType>
    {
        /// <summary>
        /// Gets or sets the amount of remaining resources in the country.
        /// </summary>
        public long RemainingAmount { get; set; }
    }
}