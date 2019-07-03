using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.DTO.Sent
{
    public class EventInfo
    {
        public int ReportId { get; set; }
        public ulong Round { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
        public bool IsSeen { get; set; }
    }
}
