namespace StrategyGame.Bll
{
    /// <summary>
    /// A class that contains known values, constants within the game and database.
    /// </summary>
    public class KnownValues
    {
        /// <summary>
        /// An effect that increases the population by the set amount. Value is an integer.
        /// </summary>
        public const string PopulationIncrease = "population-increase";

        /// <summary>
        /// An effect that increases the barrack space by the set amount. Value is integer.
        /// </summary>
        public const string BarrackSpaceIncrease = "barrack-space-increase";

        /// <summary>
        /// An effect that increases end-of-turn coral production by the set amount. Value is an integer.
        /// </summary>
        public const string CoralProductionIncrease = "coral-production";

        /// <summary>
        /// An effect that increases end-of-turn coral production by the set amount for every building of a type. 
        /// Value is an integer, target ID is the target building's ID.
        /// </summary>
        public const string BuildingProductionIncrease = "building-coral-production";

        /// <summary>
        /// An effect that increases end-of-turn pearl production by the set amount. Value is an integer.
        /// </summary>
        public const string PearlProductionIncrease = "pearl-production";

        /// <summary>
        /// An effect that increases the attack power of units by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string UnitAttackModifier = "unit-attack";

        /// <summary>
        /// An effect that increases the defense power of units by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string UnitDefenseModifier = "unit-defense";

        /// <summary>
        /// An effect that increases the taxation (pearl production) by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string TaxationModifier = "taxation-modifier";

        /// <summary>
        /// An effect that increases the taxation (pearl production) by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string HarvestModifier = "harvest-modifier";
        
        /// <summary>
        /// An effect that adds a number of a building to the country. 
        /// Value is integer, the count of buildings, target is the building type's ID.
        /// </summary>
        public const string AddBuildingEffect = "add-building";

        /// <summary>
        /// The effect that increases unit attack by a set value. Value is integer.
        /// </summary>
        public const string IncreaseUnitAttack = "increase-attack";

        /// <summary>
        /// The default time to build buildings, in turns.
        /// </summary>
        public const int DefaultBuildingTime = 5;

        /// <summary>
        /// The default time to research things, in turns.
        /// </summary>
        public const int DefaultResearchTime = 15;
    }
}