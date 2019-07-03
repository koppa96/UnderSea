namespace StrategyGame.Model.Entities.Effects
{
    /// <summary>
    /// Represents the linking table between <see cref="RandomEvent"/>s and effects.
    /// </summary>
    public class EventEffect : AbstractEntity<EventEffect>
    {
        /// <summary>
        /// Gets or sets the parent event of the effect.
        /// </summary>
        public virtual RandomEvent Event { get; set; }

        /// <summary>
        /// Gets or sets the effect of the event.
        /// </summary>
        public virtual Effect Effect { get; set; }
    }
}