using StrategyGame.Bll.Dto.Sent.Country;
using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Sent
{
    public class CommandInfo
    {
        public int Id { get; set; }
        public int TargetCountryId { get; set; }
        public string TargetCountryName { get; set; }
        public IEnumerable<BriefUnitInfo> Units { get; set; }
    }
}
