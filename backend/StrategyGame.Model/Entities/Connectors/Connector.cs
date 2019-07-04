namespace StrategyGame.Model.Entities.Connectors
{
    /// <summary>
    /// Provides a connector between two entities.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <typeparam name="TChild">The type of the child entity</typeparam>
    public class Connector<TParent, TChild> : AbstractEntity<Connector<TParent, TChild>>
        where TParent : AbstractEntity<TParent> where TChild : AbstractEntity<TChild>
    {
        /// <summary>
        /// Gets or sets the parenty entity.
        /// </summary>
        public TParent Parent { get; set; }

        /// <summary>
        /// Gets or sets the children entity.
        /// </summary>
        public TChild Child { get; set; }
    }
}