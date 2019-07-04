using StrategyGame.Model.Entities.Effects;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Creations
{
    /// <summary>
    /// Represents a research type in the UnderSea database.
    /// </summary>
    public class ResearchType : AbstractCreationType<ResearchType, ResearchResource, ResearchContent>
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
        public virtual ICollection<ResearchEffect> Effects { get; set; }

        /// <summary>
        /// Gets the collection of researches of this type that are completed.
        /// </summary>
        public virtual ICollection<CountryResearch> CompletedResearches { get; set; }

        /// <summary>
        /// Gets the collection of researches of this type that are being researched.
        /// </summary>
        public virtual ICollection<InProgressResearch> InProgressResearches { get; set; }

        public ICollection<ReportResearch> ReportResearches { get; set; }
    }
}