using System;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a research type in the UnderSea database.
    /// </summary>
    public class ResearchType : AbstractEntity<ResearchType>
    {
        /// <summary>
        /// Gets or sets the amount of pearls the research costs.
        /// </summary>
        public int CostPearl { get; set; }

        /// <summary>
        /// Gets or sets the amount of corals the research costs.
        /// </summary>
        public int CostCoral { get; set; }

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
        /// Gets the collection of effects this research provides.
        /// </summary>
        public virtual ICollection<AbstractEffect> Effects { get; protected internal set; }

        /// <summary>
        /// Gets the collection of researches of this type that are completed.
        /// </summary>
        public virtual ICollection<CountryResearch> CompletedResearches { get; protected internal set; }

        /// <summary>
        /// Gets the collection of researches of this type that are being researched.
        /// </summary>
        public virtual ICollection<InProgressResearch> InProgressResearches { get; protected internal set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchType"/>.
        /// </summary>
        /// <param name="costPearl">The amount of pearls the research costs.</param>
        /// <param name="costCoral">The amount of corals the research costs.</param>
        /// <param name="researchTime">The built time of the research (in turns).</param>
        /// <param name="maxCompletedAmount">The times the research can be completed by a single country.</param>
        /// <exception cref="ArgumentException">Thrown if the research time was negative.</exception>
        public ResearchType(int costPearl, int costCoral, int researchTime, int maxCompletedAmount = 1)
        {
            if (researchTime < 0)
            {
                throw new ArgumentException("The research time may not be negative.", nameof(researchTime));
            }

            CostPearl = costPearl;
            CostCoral = costCoral;
            ResearchTime = researchTime;
            MaxCompletedAmount = maxCompletedAmount;
        }
    }
}
