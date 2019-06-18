using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the data about what researches a country has in the UnderSea database.
    /// </summary>
    /// <remarks>
    /// This is effectively a linking table between <see cref="Country"/> and <see cref="ResearchType"/>,
    /// with an additional <see cref="Count"/> property.
    /// </remarks>
    public class CountryResearch : AbstractEntity<CountryResearch>
    {
        /// <summary>
        /// Gets the parent country this <see cref="CountryResearch"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; set; }

        /// <summary>
        /// Gets the research this <see cref="CountryResearch"/> represents within the country.
        /// </summary>
        public ResearchType Research { get; set; }

        /// <summary>
        /// Gets the amount the research was completed by the country.
        /// </summary>
        public int Count { get; set; }



        /// <summary>
        /// Initializes a new instance of <see cref="CountryResearch"/>.
        /// </summary>
        public CountryResearch()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryResearch"/>.
        /// </summary>
        /// <param name="parentCountry">The ID of the parent country this <see cref="CountryResearch"/> belongs to.</param>
        /// <param name="research">The ID of the research this <see cref="CountryResearch"/> represents within the country.</param>
        /// <param name="count">The initial amount the research was completed by the country.</param>
        /// <exception cref="ArgumentException">Thrown if count was negative.</exception>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public CountryResearch(Country parentCountry, ResearchType research, int count = 1)
        {
            if (count < 0)
            {
                throw new ArgumentException("The research count may not be negative.", nameof(count));
            }

            ParentCountry = parentCountry ?? throw new ArgumentNullException(nameof(parentCountry));
            Research = research ?? throw new ArgumentNullException(nameof(research));
            Count = count;
        }
    }
}