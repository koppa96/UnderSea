using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Resources
{
    public class UnitResource : AbstractResourceConnector<UnitType>
    {
        public int MaintenanceAmount { get; set; }
    }
}
