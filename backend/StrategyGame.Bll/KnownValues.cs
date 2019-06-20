namespace StrategyGame.Bll
{
    // To create a migration for your local DB:
    //	- Open the SQL Server object explorer
    //	- Add a new database under "Databases", by the name "UnderSeaDb"
    //	- Open the nuget package manager console(VS -> Tools)
    //	- Select the Dal project as Default project in the console
    //	- add-migration initial -context UnderSeaDatabase
    //	- update-database -context UnderSeaDatabase

    // To fill the db with initial data:
    //	- Run the DatabaseHelper project: By default this clears the entire DB, and then adds defaults.
    // Change the main method to modify the behaviour.

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
    }
}