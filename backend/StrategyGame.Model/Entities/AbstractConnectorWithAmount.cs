namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Provides a connector between two entities, and stores the amount of <see cref="TChild"/> entities in the relation.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <typeparam name="TChild">The type of the child entity</typeparam>
    public abstract class AbstractConnectorWithAmount<TParent, TChild> : AbstractConnector<TParent, TChild>
        where TParent : AbstractEntity<TParent>
        where TChild : AbstractEntity<TChild>
    {
        /// <summary>
        /// Gets or sets the amount in of <see cref="TChild"/>ren in the relation.
        /// </summary>
        public long Amount { get; set; }
    }
}