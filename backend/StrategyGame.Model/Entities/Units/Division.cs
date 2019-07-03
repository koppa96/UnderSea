namespace StrategyGame.Model.Entities.Units
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
        /// Gets the command the division belongs to. May be null if the division belongs to a report.
        /// </summary>
        public virtual Command ParentCommand { get; set; }

        /// <summary>
        /// Gets the amount of units in the division.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets the amount of battles the division took part in since the last battle.
        /// </summary>
        public int BattleCount { get; set; }
    }
}