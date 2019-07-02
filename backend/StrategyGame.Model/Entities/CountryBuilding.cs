namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the data about what buildings a country has in the UnderSea database.
    /// </summary>
    public class CountryyResource : AbstractEntity<CountryyResource>
    {
        /// <summary>
        /// Gets the parent country this <see cref="CountryyResource"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the building this <see cref="CountryyResource"/> represents within the country.
        /// </summary>
        public BuildingType Building { get; set; }

        /// <summary>
        /// Gets the amount of buildings there are in the country.
        /// </summary>
        public int Count { get; set; }
    }
}