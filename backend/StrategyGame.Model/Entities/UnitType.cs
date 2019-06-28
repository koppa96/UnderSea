using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a unit type within the UnderSea database.
    /// </summary>
    public class UnitType : AbstractEntity<UnitType>
    {
        /// <summary>
        /// Gets or sets the attack power of the unit.
        /// </summary>
        public int AttackPower { get; set; }

        /// <summary>
        /// Gets or sets the defense power of the unit.
        /// </summary>
        public int DefensePower { get; set; }

        public IEnumerable<UnitResource> Cost { get; set; }

        public IEnumerable<UnitResource> Maintenance { get; set; }

        /// <summary>
        /// Gets or sets the content of the unit.
        /// </summary>
        public virtual UnitContent Content { get; set; }

        /// <summary>
        /// Gets or sets the collection of divisions that contain the unit type.
        /// </summary>
        public virtual ICollection<Division> ContainingDivisions { get; set; } = new HashSet<Division>();

        public int BattlesToLevelUp { get; set; }

        public bool IsPurchasable { get; set; }

        public UnitType RankedUpType { get; set; }

        [NotMapped]
        public bool CanRankUp => RankedUpType != null;
    }
}