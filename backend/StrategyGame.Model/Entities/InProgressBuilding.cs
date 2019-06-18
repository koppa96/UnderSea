using System;

namespace StrategyGame.Model.Entities
{
    /// <summary>
    /// Represents a building that is currently being built in a country within the UnderSea database.
    /// </summary>
    public class InProgressBuilding : AbstractEntity<InProgressBuilding>
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
        /// Gets the amount of turns left until the building is built.
        /// </summary>
        public int TimeLeft { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="InProgressBuilding"/>.
        /// </summary>
        /// <param name="parentCountry">The parent country this <see cref="CountryBuilding"/> belongs to.</param>
        /// <param name="building">The building this <see cref="CountryBuilding"/> represents within the country.</param>
        /// <param name="count">The initial amount of buildings there are in the country.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public InProgressBuilding(Country parentCountry, BuildingType building)
        {
            ParentCountry = parentCountry ?? throw new ArgumentNullException(nameof(parentCountry));
            Building = building ?? throw new ArgumentNullException(nameof(building));

            TimeLeft = Building.BuildTime;
        }
    }
}