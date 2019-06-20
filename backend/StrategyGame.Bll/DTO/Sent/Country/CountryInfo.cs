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
        public int Round { get; set; }
        public int Rank { get; set; }
        public IEnumerable<UnitInfo> ArmyInfo { get; set; }
        public int Pearls { get; set; }
        public int Corals { get; set; }
        public BriefCreationInfo Buildings { get; set; }
        public BriefCreationInfo Researches { get; set; }
    }
}
