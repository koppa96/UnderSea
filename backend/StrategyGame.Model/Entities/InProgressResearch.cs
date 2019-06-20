namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a building that is currently being built in a country within the UnderSea database.
    /// </summary>
    public class InProgressResearch : AbstractEntity<InProgressResearch>
    {
        /// <summary>
        /// Gets the parent country this <see cref="InProgressResearch"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the research this <see cref="InProgressResearch"/> represents within the country.
        /// </summary>
        public ResearchType Research { get; set; }

        /// <summary>
        /// Gets the amount of turns left until the building is built.
        /// </summary>
        public int TimeLeft { get; set; }
    }
}