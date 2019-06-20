namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the data about what researches a country has in the UnderSea database.
    /// </summary>
    public class CountryResearch : AbstractEntity<CountryResearch>
    {
        /// <summary>
        /// Gets the parent country this <see cref="CountryResearch"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the research this <see cref="CountryResearch"/> represents within the country.
        /// </summary>
        public ResearchType Research { get; set; }

        /// <summary>
        /// Gets the amount the research was completed by the country.
        /// </summary>
        public int Count { get; set; }
    }
}