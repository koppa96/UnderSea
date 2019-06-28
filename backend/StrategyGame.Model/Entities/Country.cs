using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a country in the UnderSea database.
    /// </summary>
    public class Country : AbstractEntity<Country>
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the current amount of pearls the country has.
        /// </summary>
        public long Pearls { get; set; }

        /// <summary>
        /// Gets or sets the current amount of corals the country has.
        /// </summary>
        public long Corals { get; set; }

        /// <summary>
        /// Gets or sets the current score of the country.
        /// </summary>
        public long Score { get; set; }

        /// <summary>
        /// Gets or sets the current ranking place of the country.
        /// </summary>
        public long Rank { get; set; }

        /// <summary>
        /// Gets or sets the turn when the country was created.
        /// </summary>
        public ulong CreatedRound { get; set; }

        /// <summary>
        /// Gets the user to whom the country belongs.
        /// </summary>
        public virtual User ParentUser { get; set; }

        /// <summary>
        /// Gets or sets the event that has happened in the country at the end of the last turn.
        /// </summary>
        public virtual RandomEvent CurrentEvent { get; set; }

        /// <summary>
        /// Gets the collection of buildings that are within the country.
        /// </summary>
        public virtual ICollection<CountryBuilding> Buildings { get; set; }

        /// <summary>
        /// Gets the collection of researches completed by the country.
        /// </summary>
        public virtual ICollection<CountryResearch> Researches { get; set; }

        /// <summary>
        /// Gets the collection of buildings that are within the country.
        /// </summary>
        public virtual ICollection<InProgressBuilding> InProgressBuildings { get; set; }

        /// <summary>
        /// Gets the collection of researches completed by the country.
        /// </summary>
        public virtual ICollection<InProgressResearch> InProgressResearches { get; set; }

        /// <summary>
        /// Gets the collection of commands issues by the country for the current turn.
        /// </summary>
        public virtual ICollection<Command> Commands { get; set; }

        /// <summary>
        /// Gets the collection of commands targetting the country.
        /// </summary>
        public virtual ICollection<Command> IncomingAttacks { get; set; }

        /// <summary>
        /// Gets the collection of combat reports about offensive actions of the country.
        /// </summary>
        public virtual ICollection<CombatReport> Attacks { get; set; }

        /// <summary>
        /// Gets the collection of combat reports about defensive actions of the country.
        /// </summary>
        public virtual ICollection<CombatReport> Defenses { get; set; }
    }
}