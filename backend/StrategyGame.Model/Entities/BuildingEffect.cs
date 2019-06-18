using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities
{
    public class BuildingEffect : AbstractEntity<BuildingEffect>
    {
        public BuildingType Building { get; set; }
        public AbstractEffect Effect { get; set; }
    }
}
