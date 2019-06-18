using StrategyGame.Model.Entities.Frontend;
using System.Collections.Generic;

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

        /// <summary>
        /// Gets or sets the amount of pearls the unit costs.
        /// </summary>
        public int CostPearl { get; set; }

        /// <summary>
        /// Gets or sets the amount of corals the unit costs.
        /// </summary>
        public int CostCoral { get; set; }

        /// <summary>
        /// Gets or sets the maintenance of the unit in pearls.
        /// </summary>
        public int MaintenancePearl { get; set; }

        /// <summary>
        /// Gets or sets the maintenance of the unit in corals.
        /// </summary>
        public int MaintenanceCoral { get; set; }

        /// <summary>
        /// Gets or sets the content of the unit.
        /// </summary>
        public virtual UnitContent Content { get; set; }

        /// <summary>
        /// Gets or sets the collection of divisions that contain the unit type.
        /// </summary>
        public virtual ICollection<Division> ContainingDivisions { get; set; }



        /// <summary>
        /// Initializes a new instance of <see cref="UnitType"/>.
        /// </summary>
        public UnitType()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitType"/>.
        /// </summary>
        /// <param name="attackPower">The attack power of the unit.</param>
        /// <param name="defensePower">The defense power of the unit.</param>
        /// <param name="costPearl">The amount of pearls the unit costs.</param>
        /// <param name="costCoral">The amount of corals the unit costs.</param>
        /// <param name="maintenancePearl">The maintenance of the unit in pearls.</param>
        /// <param name="maintenanceCoral">The maintenance of the unit in corals.</param>
        public UnitType(int attackPower, int defensePower, int costPearl, int costCoral, 
            int maintenancePearl, int maintenanceCoral)
        {
            AttackPower = attackPower;
            DefensePower = defensePower;
            CostPearl = costPearl;
            CostCoral = costCoral;
            MaintenancePearl = maintenancePearl;
            MaintenanceCoral = maintenanceCoral;
        }
    }
}