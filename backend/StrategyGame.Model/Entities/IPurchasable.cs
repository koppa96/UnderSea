using StrategyGame.Model.Entities.Connectors;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Provides an interface for entities that can be purchased for a set amount of resources.
    /// </summary>
    /// <typeparam name="TEntity">The entity that can be purchased.</typeparam>
    public interface IPurchasable<TEntity>
        where TEntity : AbstractEntity<TEntity>
    {
        /// <summary>
        /// Gets or sets the collection of resources the entity costs.
        /// </summary>
        ICollection<CreationResourceConnector<TEntity>> Cost { get; set; }
    }
}