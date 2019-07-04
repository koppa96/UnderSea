using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Units
{
    public class SpyCommand : AbstractEntity<SpyCommand>
    {
        public Country ParentCountry { get; set; }
        public Country TargetCountry { get; set; }
        public int SpyAmount { get; set; }
    }
}
