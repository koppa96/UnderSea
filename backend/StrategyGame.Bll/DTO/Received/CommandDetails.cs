using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Received
{
    public class CommandDetails
    {
        public int TargetCountryId { get; set; }
        public IEnumerable<UnitDetails> Units { get; set; }
    }
}