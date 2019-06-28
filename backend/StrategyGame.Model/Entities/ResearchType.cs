using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a research type in the UnderSea database.
    /// </summary>
    public class ResearchType : AbstractEntity<ResearchType>
    {
        public ICollection<ResearchResource> Cost { get; set; }

        /// <summary>
        /// Gets or sets the built time of the research (in turns).
        /// </summary>
        public int ResearchTime { get; set; }

        /// <summary>
        /// Gets or sets the times the research can be completed by a single country.
        /// </summary>
        /// <remarks>
        /// 0 means the research can't be completed, negative means it may be completed unlimited times.
        /// </remarks>
        public int MaxCompletedAmount { get; set; }

        /// <summary>
        /// Gets or sets the content of the research.
        /// </summary>
        public virtual ResearchContent Content { get; set; }

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
    }
}