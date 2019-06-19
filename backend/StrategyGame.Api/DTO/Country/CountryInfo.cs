using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.DTO.Country
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
        public CreationInfo Buildings { get; set; }
        public CreationInfo Researches { get; set; }
    }
}
