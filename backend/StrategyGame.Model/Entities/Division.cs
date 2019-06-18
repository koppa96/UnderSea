using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a division, a group of units of the same type, within the UnderSea database.
    /// </summary>
    public class Division : AbstractEntity<Division>
    {
        /// <summary>
        /// Gets the type of units in the division.
        /// </summary>
        public virtual UnitType Unit { get; protected internal set; }

        /// <summary>
        /// Gets the command the division belongs to.
        /// </summary>
        public virtual Command ParentCommand { get; protected internal set; }

        /// <summary>
        /// Gets the amount of units in the division.
        /// </summary>
        public int Count { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Division"/>.
        /// </summary>
        /// <param name="unit">The type of units in the division.</param>
        /// <param name="parentCommand">The command the division belongs to.</param>
        /// <param name="count">The amount of units in the division.</param>
        /// <exception cref="ArgumentException">Thrown if the unit count was negative.</exception>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public Division(UnitType unit, Command parentCommand, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Unit count may not be negative.", nameof(count));
            }

            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            ParentCommand = parentCommand ?? throw new ArgumentNullException(nameof(parentCommand));
            Count = count;
        }
    }
}