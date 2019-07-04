using StrategyGame.Model.Entities.Resources;
using StrategyGame.Model.Entities.Units;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StrategyGame.Model.Entities.Creations;

namespace StrategyGame.Model.Entities.Reports
{
    /// <summary>
    /// Represents a combat report in the UnderSeaDatabase.
    /// </summary>
    public class CombatReport : AbstractEntity<CombatReport>
    {
        /// <summary>
        /// Gets or sets the country that attacked in the combat.
        /// </summary>
        public Country Attacker { get; set; }

        /// <summary>
        /// Gets or sets if the attacker has seen the report.
        /// </summary>
        public bool IsSeenByAttacker { get; set; }

        /// <summary>
        /// Gets or sets if the attacker deleted the report.
        /// </summary>
        public bool IsDeletedByAttacker { get; set; }

        /// <summary>
        /// Gets or sets the country that defended in the combat.
        /// </summary>
        public Country Defender { get; set; }

        /// <summary>
        /// Gets or sets if the defender has seen the report.
        /// </summary>
        public bool IsSeenByDefender { get; set; }

        /// <summary>
        /// Gets or sets if the defender deleted the report.
        /// </summary>
        public bool IsDeletedByDefender { get; set; }

        /// <summary>
        /// Gets or sets if the attacker won the combat.
        /// </summary>
        [NotMapped]
        public bool DidAttackerWin => TotalAttackPower > TotalDefensePower;

        /// <summary>
        /// Gets or sets the divisions that participated in the attack.
        /// </summary>
        public ICollection<Division> Attackers { get; set; }

        /// <summary>
        /// Gets or sets the divisions that defended in the attack.
        /// </summary>
        public ICollection<Division> Defenders { get; set; }

        /// <summary>
        /// Gets or sets the losses suffered by the attacker.
        /// </summary>
        public ICollection<Division> AttackerLosses { get; set; }

        /// <summary>
        /// Gets or sets the losses suffered by the defender.
        /// </summary>
        public ICollection<Division> DefenderLosses { get; set; }

        /// <summary>
        /// Gets or sets the base attack power.
        /// </summary>
        public double BaseAttackPower { get; set; }

        /// <summary>
        /// Gets or sets the attack modifier.
        /// </summary>
        public double AttackModifier { get; set; }

        /// <summary>
        /// Gets or sets the total attackpower.
        /// </summary>
        public double TotalAttackPower { get; set; }

        /// <summary>
        /// Gets or sets the base defense power.
        /// </summary>
        public double BaseDefensePower { get; set; }

        /// <summary>
        /// Gets or sets the defense modifier.
        /// </summary>
        public double DefenseModifier { get; set; }

        /// <summary>
        /// Gets or sets the total defensepower.
        /// </summary>
        public double TotalDefensePower { get; set; }

        /// <summary>
        /// Gets or sets the round when the combat happened.
        /// </summary>
        public ulong Round { get; set; }

        /// <summary>
        /// Gets or sets the collection of looted resources.
        /// </summary>
        public ICollection<ReportResource> Loot { get; set; }

        public ICollection<ReportBuilding> DefenderBuildings { get; set; }

        public ICollection<ReportResearch> DefenderResearches { get; set; }
    }
}