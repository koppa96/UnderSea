using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    public interface IPurchasable<TEntity, TConnector>
        where TEntity : AbstractEntity<TEntity>
        where TConnector : AbstractConnectorWithAmount<TEntity, ResourceType>
    {
        ICollection<TConnector> Cost { get; set; }
    }
}