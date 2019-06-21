namespace StrategyGame.Bll.Services.TurnHandling
{
    /// <summary>
    /// Provides a container for temporary, modifiable information about a country.
    /// </summary>
    public class CountryModifierBuilder
    {
        /// <summary>
        /// Gets or sets the current population of the country.
        /// </summary>
        public int Population { get; set; }

        /// <summary>
        /// Gets or sets the maximal amount of units a country may have.
        /// </summary>
        public int BarrackSpace { get; set; }

        /// <summary>
        /// Gets or sets the attack modifier for units of the country.
        /// </summary>
        public double AttackModifier { get; set; } = 1;

        /// <summary>
        /// Gets or sets the defense modifier for units of the country.
        /// </summary>
        public double DefenseModifier { get; set; } = 1;

        /// <summary>
        /// Gets or sets the tax (pearl production) modifier for the country.
        /// </summary>
        public double TaxModifier { get; set; } = 1;

        /// <summary>
        /// Gets or sets the coral production modifier for the country.
        /// </summary>
        public double HarvestModifier { get; set; } = 1;

        /// <summary>
        /// Gets or sets the country's base coral production.
        /// </summary>
        public int CoralProduction { get; set; }

        /// <summary>
        /// Gets or sets the pearl loot the country acquired.
        /// </summary>
        public long CurrentPearlLoot { get; set; }

        /// <summary>
        /// Gets or sets the coral loot the country acquired.
        /// </summary>
        public long CurrentCoralLoot { get; set; }
    }
}