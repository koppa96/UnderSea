using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the linking table between <see cref="BuildingType"/>s and effects.
    /// </summary>
    public class BuildingEffect : AbstractEntity<BuildingEffect>
    {
        /// <summary>
        /// Gets or sets the <see cref="BuildingType"/> the effect belongs.
        /// </summary>
        public BuildingType Building { get; set; }

        /// <summary>
        /// Gets or sets the effect of the building.
        /// </summary>
        public Effect Effect { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingEffect"/>.
        /// </summary>
        public BuildingEffect()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingEffect"/>.
        /// </summary>
        /// <param name="building">The <see cref="BuildingType"/> the effect belongs.</param>
        /// <param name="effect">The effect of the building.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public BuildingEffect(BuildingType building, Effect effect)
        {
            Building = building ?? throw new ArgumentNullException(nameof(building));
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
        }
    }
}