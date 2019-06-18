using System;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a command for the army of a country.
    /// </summary>
    /// <remarks>
    /// If the <see cref="ParentCountry"/> and <see cref="TargetCountry"/> are the same, the units are defending.
    /// </remarks>
    public class Command : AbstractEntity<Command>
    {
        /// <summary>
        /// Gets the country that this command belongs to.
        /// </summary>
        public virtual Country ParentCountry { get; protected internal set; }

        /// <summary>
        /// Gets the target country of the command.
        /// </summary>
        public virtual Country TargetCountry { get; protected internal set; }

        /// <summary>
        /// Gets the collection of divisions assigned to this command.
        /// </summary>
        public virtual ICollection<Division> Divisons { get; protected internal set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/>
        /// </summary>
        /// <param name="parentCountry">The country that this command belongs to.</param>
        /// <param name="targetCountry">The target country of the command.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public Command(Country parentCountry, Country targetCountry)
        {
            ParentCountry = parentCountry ?? throw new ArgumentNullException(nameof(parentCountry));
            TargetCountry = targetCountry ?? throw new ArgumentNullException(nameof(targetCountry));
        }
    }
}