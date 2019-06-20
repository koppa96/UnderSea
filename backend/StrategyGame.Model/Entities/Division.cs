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
        public virtual UnitType Unit { get; set; }

        /// <summary>
        /// Gets the command the division belongs to.
        /// </summary>
        public virtual Command ParentCommand { get; set; }

        /// <summary>
        /// Gets the amount of units in the division.
        /// </summary>
        public int Count { get; set; }
    }
}