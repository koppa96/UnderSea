using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Resources
{
    public class AbstractResourceConnector<TEntity> : AbstractEntity<AbstractResourceConnector<TEntity>> 
        where TEntity : AbstractEntity<TEntity>
    {
        public TEntity Entity { get; set; }
        public ResourceType ResourceType { get; set; }
        public long Amount { get; set; }
    }
}
