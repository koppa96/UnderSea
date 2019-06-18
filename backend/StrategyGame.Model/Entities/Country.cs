using System;
using System.Collections.Generic;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a country in the UnderSea database.
    /// </summary>
    public class Country : AbstractEntity<Country>
    {
        /// <summary>
        /// Gets or sets the current amount of pearls the country has.
        /// </summary>
        public int Pearls { get; set; }

        /// <summary>
        /// Gets or sets the current amount of corals the country has.
        /// </summary>
        public int Corals { get; set; }

        /// <summary>
        /// Gets the user to whom the country belongs.
        /// </summary>
        public virtual User ParentUser { get; set; }

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
        /// Initializes a new instance of <see cref="Country"/>.
        /// </summary>
        public Country()
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Country"/>, with the specified starting pearl and coral amounts.
        /// </summary>
        /// <param name="parentUser">The user to whom the country belongs.</param>
        /// <param name="initialPearls">The initial amount of pearls the country has.</param>
        /// <param name="initialCorals">The initial amount of corals the country has.></param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public Country(User parentUser, int initialPearls, int initialCorals)
        {
            ParentUser = parentUser ?? throw new ArgumentNullException(nameof(parentUser));
            Pearls = initialPearls;
            Corals = initialCorals;
        }
    }
}