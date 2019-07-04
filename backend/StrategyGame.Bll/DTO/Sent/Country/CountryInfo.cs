using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Sent.Country
{
    public class CountryInfo
    {
        public ulong Round { get; set; }
        public int Rank { get; set; }
        public IEnumerable<BriefUnitInfo> ArmyInfo { get; set; }
        public long Pearls { get; set; }
        public long Corals { get; set; }
        public long PearlsPerRound { get; set; }
        public long CoralsPerRound { get; set; }
        public long BarrackSpace { get; set; }
        public EventInfo Event { get; set; }
        public int UnseenReports { get; set; }
        public IEnumerable<BriefCreationInfo> Buildings { get; set; }
        public IEnumerable<BriefCreationInfo> Researches { get; set; }
    }
}