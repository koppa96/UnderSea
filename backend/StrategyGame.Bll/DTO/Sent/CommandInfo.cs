using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Dto.Sent
{
    public class CommandInfo
    {
        public int Id { get; set; }
        public int TargetCountryId { get; set; }
        public string TargetCountryName { get; set; }
        public IEnumerable<UnitInfo> Units { get; set; }
    }
}
