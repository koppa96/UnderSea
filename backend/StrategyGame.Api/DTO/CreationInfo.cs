using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Api.DTO
{
    public class CreationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
    }
}
