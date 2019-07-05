using StrategyGame.Model.Entities.Connectors;
using StrategyGame.Model.Entities.Frontend;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities.Creations
{
    public abstract class AbstractCreationType<TEntity> 
            : AbstractEntity<TEntity>, IPurchasable<TEntity>
        where TEntity : AbstractEntity<TEntity>
    {
        public FrontendContent<TEntity> Content { get; set; }
        public ICollection<CreationResourceConnector<TEntity>> Cost { get; set; }
    }
}
