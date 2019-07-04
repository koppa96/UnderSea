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
        public IEnumerable<BriefUnitInfo> YourUnits { get; set; }
        public IEnumerable<BriefUnitInfo> EnemyUnits { get; set; }
        public IEnumerable<BriefUnitInfo> YourLostUnits { get; set; }
        public IEnumerable<BriefUnitInfo> EnemyLostUnits { get; set; }
        public IEnumerable<ResourceInfo> Loot { get; set; }
        public IEnumerable<ResourceInfo> RemainingResources { get; set; }
        public IEnumerable<BriefCreationInfo> Buildings { get; set; }
        public IEnumerable<BriefCreationInfo> Researches { get; set; }
        public bool IsSeen { get; set; }
    }
}