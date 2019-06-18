using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities
{
    public class GlobalValue
    {
        //TODO comment
        public int Id { get; set; }
        public uint Round { get; set; }
        public int StartingPopulation { get; set; }
        public int StartingSoldierCapacity { get; set; }
        public int StartingPearls { get; set; }
        public int StartingCoral { get; set; }
        public int PealPerPopulation { get; set; }
    }
}
