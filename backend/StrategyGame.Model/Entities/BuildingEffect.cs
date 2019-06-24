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
    }
}