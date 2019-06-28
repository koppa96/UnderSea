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
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Count { get; set; }
        public IEnumerable<ResourceInfo> Cost { get; set; }
        public IEnumerable<ResourceInfo> Maintenance { get; set; }
    }
}