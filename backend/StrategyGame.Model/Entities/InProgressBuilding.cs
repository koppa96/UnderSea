namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a building that is currently being built in a country within the UnderSea database.
    /// </summary>
    public class InProgressBuilding : AbstractEntity<InProgressBuilding>
    {
        /// <summary>
        /// Gets the parent country this <see cref="CountryBuilding"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the building this <see cref="CountryBuilding"/> represents within the country.
        /// </summary>
        public BuildingType Building { get; set; }

        /// <summary>
        /// Gets the amount of turns left until the building is built.
        /// </summary>
        public int TimeLeft { get; set; }
    }
}