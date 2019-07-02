using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Sent
{
    public class CreationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string IconImageUrl { get; set; }
        public IEnumerable<ResourceInfo> Cost { get; set; }
    }
}