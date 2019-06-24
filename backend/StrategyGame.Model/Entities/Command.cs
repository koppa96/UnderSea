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
        public virtual Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the target country of the command.
        /// </summary>
        public virtual Country TargetCountry { get; set; }

        /// <summary>
        /// Gets or sets the amount of coral loot the <see cref="ParentCountry"/> acquired as the result of the command.
        /// </summary>
        public long AcquiredCoralLoot { get; set; }

        /// <summary>
        /// Gets or sets the amount of pearl loot the <see cref="ParentCountry"/> acquired as the result of the command.
        /// </summary>
        public long AcquiredPearlLoot { get; set; }

        /// <summary>
        /// Gets the collection of divisions assigned to this command.
        /// </summary>
        public virtual ICollection<Division> Divisions { get; set; } = new HashSet<Division>();
    }
}