using System.Collections.Generic;

namespace StrategyGame.Bll.Dto.Sent
{
    public class CombatInfo
    {
        public int Id { get; set; }
        public int Round { get; set; }
        public bool IsAttack { get; set; }
        public bool IsWon { get; set; }
        public int EnemyCountryId { get; set; }
        public string EnemyCountryName { get; set; }
        public IEnumerable<UnitInfo> YourUnits { get; set; }
        public IEnumerable<UnitInfo> EnemyUnits { get; set; }
        public IEnumerable<UnitInfo> LostUnits { get; set; }
        public int PealLoot { get; set; }
        public int CoralLoot { get; set; }
        public bool IsSeen { get; set; }
    }
}