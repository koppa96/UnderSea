using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll
{
    /// <summary>
    /// Provides exntension methods for the <see cref="Country"/> entity.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Attempts to start building a new <see cref="BuildingType"/> in the country.
        /// Returns if the building can be started (depending on the maximum building count).
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabase"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="building">The <see cref="BuildingType"/> to start.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the building could be started.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if no country with the given ID exists.</exception>
        public static async Task<bool> TryStartBuildingAsync(this UnderSeaDatabase context, int countryId,
            BuildingType building, CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (building == null)
            {
                throw new ArgumentNullException(nameof(building));
            }

            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(countryId, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel).ConfigureAwait(false);

            // Check if building count + queued building count is less than max allowed
            // Shortcut on unlimited first tho
            if (building.MaxCount == 0
                || (building.MaxCount > 0
                && building.MaxCount <= country.InProgressBuildings.Where(b => b.Building.Equals(building)).Count()
                + country.Buildings.Where(b => b.Building.Equals(building)).Count()))
            {
                return false;
            }

            await context.InProgressBuildings.AddAsync(new InProgressBuilding()
            { ParentCountry = country, Building = building, TimeLeft = building.BuildTime }, cancel)
                .ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Attempts to start a new <see cref="ResearchType"/> in the country.
        /// Returns if the research can be started (depending on the maximum research count).
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabase"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="research">The <see cref="ResearchType"/> to start researching.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the research could be started.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static async Task<bool> TryStartResearchAsync(this UnderSeaDatabase context, int countryId,
            ResearchType research, CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (research == null)
            {
                throw new ArgumentNullException(nameof(research));
            }

            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(countryId, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel).ConfigureAwait(false);

            if (research.MaxCompletedAmount == 0
                || (research.MaxCompletedAmount > 0
                && research.MaxCompletedAmount <= country.InProgressResearches.Where(r => r.Research.Equals(research)).Count()
                + country.Researches.Where(r => r.Research.Equals(research)).Count()))
            {
                return false;
            }

            await context.InProgressResearches.AddAsync(new InProgressResearch()
            { ParentCountry = country, Research = research, TimeLeft = research.ResearchTime }, cancel)
                .ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Checks all in-progress buildings and researches of the country, 
        /// and adds any completed ones to it while deleting them from the in-progress ones.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabase"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the building could be started.</returns>
        /// <returns>The <see cref="Task"/> representing the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <remarks>
        /// This method does not perform any safety check regarding the amount of buildings or researches!
        /// </remarks>
        public static async Task CheckAddCompletedAsync(UnderSeaDatabase context, int countryId,
            CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(countryId, cancel).ConfigureAwait(false);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel).ConfigureAwait(false);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel).ConfigureAwait(false);

            var researches = country.InProgressResearches.Where(r => r.TimeLeft <= 0)
                .GroupBy(r => r.Research)
                .ToList();

            if (researches.Count > 0)
            {
                foreach (var research in researches)
                {
                    var existing = country.Researches.FirstOrDefault(r => r.Research.Equals(research.Key));

                    // Add a new research
                    if (existing == null)
                    {
                        await context.CountryResearches.AddAsync(new CountryResearch()
                        { ParentCountry = country, Research = research.Key, Count = research.Count() }, cancel)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        // Or update an existing one, this uses the context in order to ensure tha saving the givn context updates the db.
                        // TODO: use context to get existing?
                        (await context.CountryResearches.FindAsync(existing.Id, cancel).ConfigureAwait(false)).Count += research.Count();
                    }
                }

                // Delete all in progress ones
                context.InProgressResearches.RemoveRange(country.InProgressResearches.Where(r => r.TimeLeft <= 0));
            }

            // Get and complete buildings
            var buildings = country.InProgressBuildings.Where(r => r.TimeLeft <= 0)
                .GroupBy(b => b.Building)
                .ToList();

            if (buildings.Count > 0)
            {
                foreach (var building in buildings)
                {
                    var existing = country.Buildings.FirstOrDefault(b => b.Building.Equals(building.Key));

                    // Add a new research
                    if (existing == null)
                    {
                        await context.CountryBuildings.AddAsync(new CountryBuilding()
                        { ParentCountry = country, Building = building.Key, Count = building.Count() },
                        cancel).ConfigureAwait(false);
                    }
                    else
                    {
                        // Or update an existing one, this uses the context in order to ensure tha saving the givn context updates the db.
                        // TODO: use context to get existing?
                        (await context.CountryResearches.FindAsync(existing.Id, cancel).ConfigureAwait(false)).Count += building.Count();
                    }
                }

                // Delete all in progress ones
                context.InProgressBuildings.RemoveRange(country.InProgressBuildings.Where(b => b.TimeLeft <= 0));
            }
        }

        /// <summary>
        /// Gets all <see cref="Division"/>s tha are defending the country. THe <see cref="Country.Commands"/> collection must be loaded.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> to get the defenders for.</param>
        /// <returns>The collection of <see cref="Division"/>s defending the country.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static Command GetAllDefending(this Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            return country.Commands.Single(c => c.ParentCountry.Equals(c.TargetCountry));
        }
    }
}