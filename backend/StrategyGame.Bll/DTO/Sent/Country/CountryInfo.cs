using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Sent.Country
{
    /// <summary>
    /// Contains information about the current country.
    /// </summary>
    public class CountryInfo
    {
        public ulong Round { get; set; }
        public int Rank { get; set; }
        public IEnumerable<BriefUnitInfo> ArmyInfo { get; set; }
        public IEnumerable<ResourceInfo> Resources { get; set; }
        public long PearlsPerRound { get; set; }
        public long CoralsPerRound { get; set; }
        public EventInfo Event { get; set; }
        public int UnseenReports { get; set; }
        public IEnumerable<BriefCreationInfo> Buildings { get; set; }
        public IEnumerable<BriefCreationInfo> Researches { get; set; }
    }
}