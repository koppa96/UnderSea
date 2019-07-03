using StrategyGame.Model.Entities.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Resources
{
    public class UnitResource : AbstractConnectorWithAmount<UnitType, ResourceType>
    {
        public int MaintenanceAmount { get; set; }
    }
}
