using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Provides an interface for entities that can be purchased for a set amount of resources.
    /// </summary>
    /// <typeparam name="TEntity">The entity that can be purchased.</typeparam>
    /// <typeparam name="TConnector">The connector between the entity and a <see cref="ResourceType"/>.</typeparam>
    public interface IPurchasable<TEntity, TConnector>
        where TEntity : AbstractEntity<TEntity>
        where TConnector : AbstractConnectorWithAmount<TEntity, ResourceType>
    {
        /// <summary>
        /// Gets or sets the collection of resources the entity costs.
        /// </summary>
        ICollection<TConnector> Cost { get; set; }
    }
}