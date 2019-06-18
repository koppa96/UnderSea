using System;

namespace StrategyGame.Model.Entities.Frontend
{
    /// <summary>
    /// Represents the content for a research object.
    /// </summary>
    public class ResearchContent : AbstractFrontendContent<ResearchType, ResearchContent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchContent"/>.
        /// </summary>
        public ResearchContent()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchType"/>.
        /// </summary>
        /// <param name="parent">The research the content belongs to.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public ResearchContent(ResearchType parent)
            : base(parent)
        { }
    }
}