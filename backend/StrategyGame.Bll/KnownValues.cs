namespace StrategyGame.Bll
{
    /// <summary>
    /// A class that contains known values, constants within the game and database.
    /// </summary>
    public class KnownValues
    {
        /// <summary>
        /// An effect that increases end-of-turn resource by the set amount. 
        /// Parameter: "resource type ID";"amount (long)"
        /// </summary>
        public const string ResourceProductionChange = "resource-production-change";

        /// <summary>
        /// An effect that increases end-of-turn resource by a multiplier. 
        /// Parameter: "resource type ID";"modifier (double)"
        /// </summary>
        public const string ResourceProductionModifier = "resource-modifier";

        /// <summary>
        /// An effect that increases end-of-turn production by the set amount for every building of a type. 
        /// Parameter: "building type ID (int)";"resource type ID (int)";"amount (int)"
        /// </summary>
        public const string BuildingProductionChange = "building-production-change";

        /// <summary>
        /// An effect that increases the population by the set amount, and specifies zero, one, or more resources the population generates.
        /// Parameter: "amount (int)";"resource type ID (int)":"amount (int)";"resource type ID (int)":"amount (int)"; [...]
        /// </summary>
        public const string PopulationChange = "population-change";

        /// <summary>
        /// An effect that increases the barrack space by the set amount.
        /// Parameter: "amount (int)"
        /// </summary>
        public const string BarrackSpaceChange = "barrack-space-change";

        /// <summary>
        /// An effect that increases the attack power of units by the set percent.
        /// Parameter: "amount (double)"
        /// </summary>
        public const string UnitAttackModifier = "unit-attack";

        /// <summary>
        /// An effect that increases the defense power of units by the set percent. 
        /// Parameter: "amount (double)"
        /// </summary>
        public const string UnitDefenseModifier = "unit-defense";

        /// <summary>
        /// The effect that increases unit attack by a set value. Value is integer.
        /// Parameter: "amount (int)"
        /// </summary>
        public const string UnitAttackChange = "change-attack";

        /// <summary>
        /// The effect that creates a new country for the user when it happens.
        /// Parameter: None
        /// </summary>
        public const string NewCountryEffect = "new-country";

        /// <summary>
        /// An effect that adds a number of a building to the country. 
        /// Parameter: "building type ID (int)";"amount (int)"
        /// </summary>
        public const string AddRemoveBuildingEffect = "modify-building-count";

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