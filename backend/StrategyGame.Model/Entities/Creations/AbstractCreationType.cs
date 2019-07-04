using System;
using System.Collections.Generic;
using System.Text;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;

namespace StrategyGame.Model.Entities.Creations
{
    public abstract class AbstractCreationType<TEntity, TConnector, TContent> 
            : AbstractEntity<TEntity>, IPurchasable<TEntity, TConnector>
        where TConnector : ConnectorWithAmount<TEntity, ResourceType>
        where TContent : AbstractFrontendContent<TEntity>
        where TEntity : AbstractEntity<TEntity>
    {
        public TContent Content { get; set; }
        public ICollection<TConnector> Cost { get; set; }
    }
}
