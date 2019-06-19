namespace StrategyGame.Bll
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
        public double AttackModifier { get; set; }

        /// <summary>
        /// Gets or sets the defense modifier for units of the country.
        /// </summary>
        public double DefenseModifier { get; set; }

        /// <summary>
        /// Gets or sets the tax (pearl production) modifier for the country.
        /// </summary>
        public double TaxModifier { get; set; }

        /// <summary>
        /// Gets or sets the coral production modifier for the country.
        /// </summary>
        public double HarvestModifier { get; set; }

        /// <summary>
        /// Gets or sets the country's base coral production.
        /// </summary>
        public int CoralProduction { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="CountryModifierBuilder"/>.
        /// </summary>
        public CountryModifierBuilder()
        {
            Population = 0;
            BarrackSpace = 0;
            AttackModifier = 1;
            DefenseModifier = 1;
            TaxModifier = 1;
            HarvestModifier = 1;
            CoralProduction = 0;
        }
    }
}