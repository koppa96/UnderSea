using StrategyGame.Model.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities
{
    public interface IPurchasable<TEntity, TConnector>
        where TEntity : AbstractEntity<TEntity>
        where TConnector : AbstractResourceConnector<TEntity>
    {
        ICollection<TConnector> Cost { get; set; }
    }
}
