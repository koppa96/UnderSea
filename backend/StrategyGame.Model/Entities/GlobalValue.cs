namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the global values in the UnderSea database.
    /// </summary>
    public class GlobalValue : AbstractEntity<GlobalValue>
    {
        /// <summary>
        /// Gets or sets the current round of the game.
        /// </summary>
        public ulong Round { get; set; }

        /// <summary>
        /// Gets or sets the starting population for a country.
        /// </summary>
        public int StartingPopulation { get; set; }

        /// <summary>
        /// Gets or sets the barrack space for a country.
        /// </summary>
        public int StartingBarrackSpace { get; set; }

        /// <summary>
        /// Gets or sets the starting pearls for a country.
        /// </summary>
        public int StartingPearls { get; set; }

        /// <summary>
        /// Gets or sets the starting corals for a country.
        /// </summary>
        public int StartingCorals { get; set; }

        /// <summary>
        /// Gets or sets the base taxation (pearl production / population) of a country.
        /// </summary>
        public int BaseTaxation { get; set; }
    }
}