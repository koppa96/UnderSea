namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Provides a connector between two entities, and stores a countdown until the relationship exists.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <typeparam name="TChild">The type of the child entity</typeparam>
    public abstract class AbstractConnectorWithProgress<TParent, TChild> : AbstractConnector<TParent, TChild>
        where TParent : AbstractEntity<TParent>
        where TChild : AbstractEntity<TChild>
    {
        /// <summary>
        /// Gets or sets the time until the relationships exists.
        /// </summary>
        public int TimeLeft { get; set; }
    }
}