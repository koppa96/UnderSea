﻿namespace StrategyGame.Bll
{
    /// <summary>
    /// A class that contains known values, constants within the game and database.
    /// </summary>
    public class KnownValues
    {
        /// <summary>
        /// An effect that increases end-of-turn resource by the set amount. 
        /// Value is an integer, parameter is the ID of the resource.
        /// </summary>
        public const string ResourceProductionChange = "resource-production-change";

        /// <summary>
        /// An effect that increases end-of-turn resource by a multiplier. 
        /// Value is a double, parameter is the ID of the resource.
        /// </summary>
        public const string ResourceProductionModifier = "resource-modifier";

        /// <summary>
        /// An effect that increases end-of-turn production by the set amount for every building of a type. 
        /// Value is an integer, parameter is the target building's ID, and the resource's ID, separated by a semicolon.
        /// </summary>
        public const string BuildingProductionChange = "building-production-change";

        /// <summary>
        /// An effect that increases the population by the set amount. Value is an integer,
        /// parameter is the list of resource IDs the population makes, and the amount they make in the following way:
        /// res:amount;res:amount
        /// </summary>
        public const string PopulationChange = "population-change";

        /// <summary>
        /// An effect that increases the barrack space by the set amount. Value is integer.
        /// </summary>
        public const string BarrackSpaceChange = "barrack-space-change";
        
        /// <summary>
        /// An effect that increases the attack power of units by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string UnitAttackModifier = "unit-attack";

        /// <summary>
        /// An effect that increases the defense power of units by the set percent. Value is double where 0.5 is 50%.
        /// </summary>
        public const string UnitDefenseModifier = "unit-defense";
        
        /// <summary>
        /// An effect that adds a number of a building to the country. 
        /// Value is integer, the count of buildings, target is the building type's ID.
        /// </summary>
        public const string AddRemoveBuildingEffect = "modify-building-count";

        /// <summary>
        /// The effect that increases unit attack by a set value. Value is integer.
        /// </summary>
        public const string UnitAttackChange = "change-attack";

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