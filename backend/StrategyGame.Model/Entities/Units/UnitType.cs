using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrategyGame.Model.Entities.Units
{
    /// <summary>
    /// Represents a unit type within the UnderSea database.
    /// </summary>
    public class UnitType : AbstractEntity<UnitType>, IPurchasable<UnitType, UnitResource>
    {
        /// <summary>
        /// Gets or sets the attack power of the unit.
        /// </summary>
        public int AttackPower { get; set; }

        /// <summary>
        /// Gets or sets the defense power of the unit.
        /// </summary>
        public int DefensePower { get; set; }

        /// <summary>
        /// Gets or sets the total carry capacity of the unit.
        /// </summary>
        public int CarryCapacity { get; set; }
        
        /// <summary>
        /// Gets or sets the costs of the unit.
        /// </summary>
        public ICollection<UnitResource> Cost { get; set; }

        /// <summary>
        /// Gets or sets the content of the unit.
        /// </summary>
        public virtual UnitContent Content { get; set; }

        /// <summary>
        /// Gets or sets the collection of divisions that contain the unit type.
        /// </summary>
        public virtual ICollection<Division> ContainingDivisions { get; set; }

        /// <summary>
        /// Gets or sets the amount of battles this unit needs to level up.
        /// </summary>
        public int BattlesToRankUp { get; set; }

        /// <summary>
        /// gets or sets if the unit can be purchased.
        /// </summary>
        public bool IsPurchasable { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="UnitType"/> of the next level.
        /// </summary>
        public UnitType RankedUpType { get; set; }

        /// <summary>
        /// Gets if the unit can level up.
        /// </summary>
        [NotMapped]
        public bool CanRankUp => RankedUpType != null;
    }
}