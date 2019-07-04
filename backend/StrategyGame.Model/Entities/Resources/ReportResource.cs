using StrategyGame.Model.Entities.Reports;

namespace StrategyGame.Model.Entities.Resources
{
    /// <summary>
    /// Linking table between a report and a resource.
    /// </summary>
    public class ReportResource : AbstractConnectorWithAmount<CombatReport, ResourceType>
    {
        public int RemainingAmount { get; set; }
    }
}