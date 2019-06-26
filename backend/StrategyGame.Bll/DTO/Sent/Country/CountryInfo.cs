using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Dto.Sent.Country
{
    /// <summary>
    /// Contains information about the current country.
    /// </summary>
    public class CountryInfo
    {
        public ulong Round { get; set; }
        public int Rank { get; set; }
        public IEnumerable<UnitInfo> ArmyInfo { get; set; }
        public long Pearls { get; set; }
        public long Corals { get; set; }
        public EventInfo Event { get; set; }
        public int UnseenReports { get; set; }
        public IEnumerable<BriefCreationInfo> Buildings { get; set; }
        public IEnumerable<BriefCreationInfo> Researches { get; set; }
    }
}
