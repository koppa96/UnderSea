using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the linking table between <see cref="ResearchType"/>s and effects.
    /// </summary>
    public class ResearchEffect : AbstractEntity<ResearchEffect>
    {
        /// <summary>
        /// Gets or sets the <see cref="ResearchType"/> the effect belongs to.
        /// </summary>
        public ResearchType Research { get; set; }
        
        /// <summary>
        /// Gets or sets the effect of the research.
        /// </summary>
        public Effect Effect { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchEffect"/>.
        /// </summary>
        public ResearchEffect()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchEffect"/>.
        /// </summary>
        /// <param name="research">The <see cref="ResearchType"/> the effect belongs to.</param>
        /// <param name="effect">The effect of the research.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public ResearchEffect(ResearchType research, Effect effect)
        {
            Research = research ?? throw new ArgumentNullException(nameof(research));
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
        }
    }
}