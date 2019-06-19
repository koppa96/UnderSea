using System;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents an effect within the UnderSea database.
    /// </summary>
    public class Effect : AbstractEntity<Effect>
    {
        /// <summary>
        /// Gets or sets the value of the effect.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of buildings that are using this effect.
        /// </summary>
        public ICollection<BuildingEffect> AffectedBuildings { get; set; }

        /// <summary>
        /// Gets or sets the collection of researches that are using this effect.
        /// </summary>
        public ICollection<ResearchEffect> AffectedResearches { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/>.
        /// </summary>
        public Effect()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/>, with initial values.
        /// </summary>
        /// <param name="name">The name of the effect.</param>
        /// <param name="value">The value of the effect.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public Effect(string name, double value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Name = name;
            Value = value;
        }
    }
}