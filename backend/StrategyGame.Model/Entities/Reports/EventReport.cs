using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Reports
{
    public class EventReport : AbstractEntity<EventReport>
    {
        public Country Country { get; set; }
        public RandomEvent Event { get; set; }
        public ulong Round { get; set; }
        public bool IsSeen { get; set; }
    }
}
