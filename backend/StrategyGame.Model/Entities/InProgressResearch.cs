using Microsoft.EntityFrameworkCore;
using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a building that is currently being built in a country within the UnderSea database.
    /// </summary>
    public class InProgressResearch : AbstractEntity<InProgressResearch>
    {
        /// <summary>
        /// Gets the parent country this <see cref="InProgressResearch"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; protected internal set; }

        /// <summary>
        /// Gets the research this <see cref="InProgressResearch"/> represents within the country.
        /// </summary>
        public ResearchType Research { get; protected internal set; }

        /// <summary>
        /// Gets the amount of turns left until the building is built.
        /// </summary>
        public int TimeLeft { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="InProgressResearch"/>.
        /// </summary>
        /// <param name="parentCountry">The parent country this <see cref="InProgressResearch"/> belongs to.</param>
        /// <param name="research">The research this <see cref="InProgressResearch"/> represents within the country.</param>
        /// <param name="count">The initial amount of buildings there are in the country.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public InProgressResearch(Country parentCountry, ResearchType research)
        {
            ParentCountry = parentCountry ?? throw new ArgumentNullException(nameof(parentCountry));
            Research = research ?? throw new ArgumentNullException(nameof(research));

            TimeLeft = Research.ResearchTime;
        }
    }
}