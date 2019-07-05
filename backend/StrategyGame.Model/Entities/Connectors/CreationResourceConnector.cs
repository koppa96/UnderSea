using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Connectors
{
    /// <summary>
    /// Provides a connection between a creation and its cost.
    /// </summary>
    /// <typeparam name="TEntity">The creation</typeparam>
    public class CreationResourceConnector<TEntity> : Connector<TEntity, ResourceType>
        where TEntity : AbstractEntity<TEntity>
    {
        public int CostAmount { get; set; }
        public int MaintenanceAmount { get; set; }
    }
}
