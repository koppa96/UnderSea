using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities
{
    public class ResearchEffect : AbstractEntity<ResearchEffect>
    {
        public ResearchType Research { get; set; }
        public AbstractEffect Effect { get; set; }
    }
}
