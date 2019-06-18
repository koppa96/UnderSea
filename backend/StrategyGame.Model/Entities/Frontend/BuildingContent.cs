using System;

namespace StrategyGame.Model.Entities.Frontend
{
    /// <summary>
    /// Represents the content for a building object.
    /// </summary>
    public class BuildingContent : AbstractFrontendContent<BuildingType, BuildingContent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingContent"/>.
        /// </summary>
        public BuildingContent()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingContent"/>.
        /// </summary>
        /// <param name="parent">The building the content belongs to.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public BuildingContent(BuildingType parent)
            : base(parent)
        { }
    }
}