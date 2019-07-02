using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Received
{
    public class TransferDetails
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public IEnumerable<ResourceAmount> Resources { get; set; }
    }
}