using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    public class CombatReport : AbstractEntity<CombatReport>
    {
        public Country Attacker { get; set; }

        public bool IsSeenByAttacker { get; set; }

        public bool IsDeletedByAttacker { get; set; }

        public Country Defender { get; set; }

        public bool IsSeenByDefender { get; set; }

        public bool IsDeletedByDefender { get; set; }

        public bool DidAttackerWin { get; set; }

        public ICollection<Division> Attackers { get; set; }

        public ICollection<Division> Defenders { get; set; }

        public ICollection<Division> Losses { get; set; }

        public double BaseAttackPower { get; set; }

        public double AttackModifier { get; set; }

        public double TotalAttackPower { get; set; }

        public double BaseDefensePower { get; set; }

        public double DefenseModifier { get; set; }

        public double TotalDefensePower { get; set; }

        public long PearlLoot { get; set; }

        public long CoralLoot { get; set; }

        public ulong Round { get; set; }
    }
}
