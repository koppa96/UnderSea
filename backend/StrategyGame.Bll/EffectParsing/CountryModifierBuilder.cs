using System.Collections.Generic;

namespace StrategyGame.Bll.EffectParsing
{
    /// <summary>
    /// Provides a container for temporary, modifiable information about a country.
    /// </summary>
    public class CountryModifierBuilder
    {
        /// <summary>
        /// Gets or sets the dictionary that contains resource producation values, by the resource's ID.
        /// </summary>
        public Dictionary<int, long> ResourceProductions { get; set; } = new Dictionary<int, long>();

        /// <summary>
        /// Gets or sets the dictionary that contains resource modifier values, by the resource's ID.
        /// </summary>
        public Dictionary<int, double> ResourceModifiers { get; set; } = new Dictionary<int, double>();

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
        /// Gets or sets the dynamic increase of unit stats.
        /// </summary>
        public int AttackIncrease { get; set; }

        /// <summary>
        /// Gets or sets the defense modifier for units of the country.
        /// </summary>
        public double DefenseModifier { get; set; } = 1;
                
        /// <summary>
        /// Gets or sets if the current event of the country was ignored because it could not be applied.
        /// </summary>
        public bool WasEventIgnored { get; set; } = false;
    }
}