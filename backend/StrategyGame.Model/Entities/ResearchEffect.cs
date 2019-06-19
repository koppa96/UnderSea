using StrategyGame.Model.Entities.Effects;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the linking table between <see cref="ResearchType"/>s and effects.
    /// </summary>
    public class ResearchEffect : AbstractEntity<ResearchEffect>
    {
        /// <summary>
        /// Gets or sets the <see cref="ResearchType"/> the effect belongs.
        /// </summary>
        public ResearchType Research { get; set; }
        
        /// <summary>
        /// Gets or sets the effect of the research.
        /// </summary>
        public Effect Effect { get; set; }
    }
}