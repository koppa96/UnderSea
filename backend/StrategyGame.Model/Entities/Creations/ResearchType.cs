using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Reports;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Creations
{
    /// <summary>
    /// Represents a research type in the UnderSea database.
    /// </summary>
    public class ResearchType : AbstractCreationType<ResearchType, ConnectorWithAmount<ResearchType, ResourceType>>
    {
        /// <summary>
        /// Gets or sets the built time of the research (in turns).
        /// </summary>
        public int ResearchTime { get; set; }

        /// <summary>
        /// Gets or sets the times the research can be completed by a single country.
        /// 0 means the research can't be completed, negative means it may be completed unlimited times.
        /// </summary>
        public int MaxCompletedAmount { get; set; }

        /// <summary>
        /// Gets the collection of effects this research provides.
        /// </summary>
        public virtual ICollection<Connector<ResearchType, Effect>> Effects { get; set; }

        /// <summary>
        /// Gets the collection of researches of this type that are completed.
        /// </summary>
        public virtual ICollection<ConnectorWithAmount<Country, ResearchType>> CompletedResearches { get; set; }

        /// <summary>
        /// Gets the collection of researches of this type that are being researched.
        /// </summary>
        public virtual ICollection<ConnectorWithProgress<Country, ResearchType>> InProgressResearches { get; set; }

        public ICollection<ConnectorWithAmount<CombatReport, ResearchType>> ReportResearches { get; set; }
    }
}