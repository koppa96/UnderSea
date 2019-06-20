using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Dto.Sent
{
    public class UnitInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Count { get; set; }
        public int PearlsPerRound { get; set; }
        public int CoralsPerRound { get; set; }
        public int Price { get; set; }
    }
}
