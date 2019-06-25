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

        /// <summary>
        /// Gets or sets the percentage of units lost in a lost battle.
        /// </summary>
        public double UnitLossOnLostBatle { get; set; }

        /// <summary>
        /// Gets or sets the percentage of resources lost when the country is looted.
        /// </summary>
        public double LootPercentage { get; set; }

        /// <summary>
        /// Gets or sets the multiplier for population during score calculation.
        /// </summary>
        public double ScorePopulationMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the multiplier for units during score calculation.
        /// </summary>
        public double ScoreUnitMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the multiplier for buildings during score calculation.
        /// </summary>
        public double ScoreBuildingMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the multiplier for researches during score calculation.
        /// </summary>
        public double ScoreResearchMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the chance for a random event to occur.
        /// </summary>
        public double RandomEventChance { get; set; }

        /// <summary>
        /// Gets or sets the amount of turns before a random event can occur.
        /// </summary>
        public ulong RandomEventGraceTimer { get; set; }

        /// <summary>
        /// Gets or sets the first starting building for a new country.
        /// </summary>
        public BuildingType FirstStartingBuilding { get; set; }

        /// <summary>
        /// Gets or sets the second starting building for a new country.
        /// </summary>
        public BuildingType SecondStartingBuilding { get; set; }
    }
}