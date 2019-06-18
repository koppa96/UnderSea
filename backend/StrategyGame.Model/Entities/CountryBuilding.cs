using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents the data about what buildings a country has in the UnderSea database.
    /// </summary>
    /// <remarks>
    /// This is effectively a linking table between <see cref="Country"/> and <see cref="BuildingType"/>,
    /// with an additional <see cref="Count"/> property.
    /// </remarks>
    public class CountryBuilding : AbstractEntity<CountryBuilding>
    {
        /// <summary>
        /// Gets the parent country this <see cref="CountryBuilding"/> belongs to.
        /// </summary>
        public Country ParentCountry { get; protected internal set; }

        /// <summary>
        /// Gets the building this <see cref="CountryBuilding"/> represents within the country.
        /// </summary>
        public BuildingType Building { get; protected internal set; }

        /// <summary>
        /// Gets the amount of buildings there are in the country.
        /// </summary>
        public int Count { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="CountryBuilding"/>.
        /// </summary>
        /// <param name="parentCountry">The parent country this <see cref="CountryBuilding"/> belongs to.</param>
        /// <param name="building">The building this <see cref="CountryBuilding"/> represents within the country.</param>
        /// <param name="count">The initial amount of buildings there are in the country.</param>
        /// <exception cref="ArgumentException">Thrown if count was negative.</exception>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public CountryBuilding(Country parentCountry, BuildingType building, int count = 1)
        {
            if (count < 0)
            {
                throw new ArgumentException("The building count may not be negative.", nameof(count));
            }

            ParentCountry = parentCountry ?? throw new ArgumentNullException(nameof(parentCountry));
            Building = building ?? throw new ArgumentNullException(nameof(building));
            Count = count;
        }
    }
}