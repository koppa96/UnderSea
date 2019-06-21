using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Extensions
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
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="building">The <see cref="BuildingType"/> to start.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the building could be started.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if no country with the given ID exists.</exception>
        public static async Task<bool> TryStartBuildingAsync(this UnderSeaDatabaseContext context, int countryId,
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
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel);

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
                ;

            return true;
        }

        /// <summary>
        /// Attempts to start a new <see cref="ResearchType"/> in the country.
        /// Returns if the research can be started (depending on the maximum research count).
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="research">The <see cref="ResearchType"/> to start researching.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the research could be started.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static async Task<bool> TryStartResearchAsync(this UnderSeaDatabaseContext context, int countryId,
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
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel);

            if (research.MaxCompletedAmount == 0
                || (research.MaxCompletedAmount > 0
                && research.MaxCompletedAmount <= country.InProgressResearches.Where(r => r.Research.Equals(research)).Count()
                + country.Researches.Where(r => r.Research.Equals(research)).Count()))
            {
                return false;
            }

            await context.InProgressResearches.AddAsync(new InProgressResearch()
            { ParentCountry = country, Research = research, TimeLeft = research.ResearchTime }, cancel)
                ;

            return true;
        }

        public static CommandInfo ToCommandInfo(this Command command, IMapper mapper)
        {
            var commandInfo = mapper.Map<Command, CommandInfo>(command);
            commandInfo.Units = command.Divisons.Select(d => {
                var unitInfo = mapper.Map<UnitType, UnitInfo>(d.Unit);
                unitInfo.Count = d.Count;
                return unitInfo;
            });

            return commandInfo;
        }

        /// <summary>
        /// Checks all in-progress buildings and researches of the country, 
        /// and adds any completed ones to it. Does not delete in progress values that are completed.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> to use.</param>
        /// <param name="countryId">The ID of the <see cref="Country"/> to build in.</param>
        /// <param name="cancel">The token that can be used to cancel the operation.</param>
        /// <returns>If the building could be started.</returns>
        /// <returns>The <see cref="Task"/> representing the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        /// <remarks>
        /// This method does not perform any safety check regarding the amount of buildings or researches!
        /// </remarks>
        public static async Task CheckAddCompletedAsync(this UnderSeaDatabaseContext context, int countryId,
            CancellationToken cancel = default)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Find the country and then load the nav properties we need
            var country = await context.Countries.FindAsync(new object[] { countryId }, cancel);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            await context.Entry(country).Collection(c => c.InProgressBuildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.InProgressResearches).LoadAsync(cancel);
            await context.Entry(country).Collection(c => c.Researches).LoadAsync(cancel);

            var researches = country.InProgressResearches.Where(r => r.TimeLeft == 0)
                .GroupBy(r => r.Research)
                .ToList();

            if (researches.Count > 0)
            {
                foreach (var research in researches)
                {
                    var existing = country.Researches.FirstOrDefault(r => r.Research.Equals(research.Key));

                    // Add a new research, or update an existing one
                    if (existing == null)
                    {
                        await context.CountryResearches.AddAsync(new CountryResearch()
                        { ParentCountry = country, Research = research.Key, Count = research.Count() }, cancel)
                            ;
                    }
                    else
                    {
                        existing.Count += research.Count();
                    }
                }
            }

            // Get and complete buildings
            var buildings = country.InProgressBuildings.Where(r => r.TimeLeft == 0)
                .GroupBy(b => b.Building)
                .ToList();

            if (buildings.Count > 0)
            {
                foreach (var building in buildings)
                {
                    var existing = country.Buildings.FirstOrDefault(b => b.Building.Equals(building.Key));

                    // Add a new building, or update an existing one
                    if (existing == null)
                    {
                        await context.CountryBuildings.AddAsync(new CountryBuilding()
                        { ParentCountry = country, Building = building.Key, Count = building.Count() },
                        cancel);
                    }
                    else
                    {
                        existing.Count += building.Count();

                    }
                }
            }
        }

        /// <summary>
        /// Gets all <see cref="Division"/>s tha are defending the country. The <see cref="Country.Commands"/> collection must be loaded.
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

        public static async Task<CountryModifierBuilder> ParseAllEffectForCountryAsync(this UnderSeaDatabaseContext context, 
            int countryId, ModifierParserContainer Parsers)
        {
            var country = await context.Countries.FindAsync(countryId);

            if (country == null)
            {
                throw new KeyNotFoundException();
            }
            
            await context.Entry(country).Collection(c => c.Buildings).LoadAsync();
            await context.Entry(country).Collection(c => c.Researches).LoadAsync();

            // Loads builngs, researches and effects
            foreach (var building in country.Buildings)
            {
                await context.Entry(building).Reference(b => b.Building).LoadAsync();
                await context.Entry(building.Building).Collection(b => b.Effects).LoadAsync();

                foreach (var effect in building.Building.Effects)
                {
                    await context.Entry(effect).Reference(e => e.Effect).LoadAsync();
                }
            }

            foreach (var research in country.Researches)
            {
                await context.Entry(research).Reference(r => r.Research).LoadAsync();
                await context.Entry(research.Research).Collection(r =>r.Effects).LoadAsync();

                foreach (var effect in research.Research.Effects)
                {
                    await context.Entry(effect).Reference(e => e.Effect).LoadAsync();
                }
            }

            var effectparents = country.Buildings.Select(b => new { count = b.Count, effects = b.Building.Effects.Select(e => e.Effect) })
                .Concat(country.Researches.Select(r => new { count = r.Count, effects = r.Research.Effects.Select(e => e.Effect) })).ToList();

            var globals = await context.GlobalValues.SingleAsync();

            // Set up builder
            var builder = new CountryModifierBuilder
            {
                BarrackSpace = globals.StartingBarrackSpace,
                Population = globals.StartingPopulation
            };

            // Calculate the effects
            foreach (var effectParent in effectparents)
            {
                for (int iii = 0; iii < effectParent.count; iii++)
                {
                    foreach (var e in effectParent.effects)
                    {
                        if (!Parsers.TryParse(e, builder))
                        {
                            Debug.WriteLine("Effect with name {0} could not be handled by the provided parsers.", e.Name);
                        }
                    }
                }
            }

            return builder;
        }

        /// <summary>
        /// Gets all <see cref="UnitInfo"/>s from all commands and divisions of a country, including units the country does not yet have.
        /// The commands, divisions, unit types and unit contents must be included in the country.
        /// </summary>
        /// <param name="country">The country to get units for.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="mapper">The mapper used to convert <see cref="UnitType"/> to <see cref="UnitInfo"/>.</param>
        /// <returns>The list of units.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static async Task<IEnumerable<UnitInfo>> GetAllUnitInfoAsync(this Country country, UnderSeaDatabaseContext context,
            IMapper mapper)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            var flattened = await context.UnitTypes
                .Include(u => u.Content)
                .ToDictionaryAsync(x => x, x => 0);

            foreach (var div in country.Commands.SelectMany(c => c.Divisons))
            {
                flattened[div.Unit] += div.Count;
            }

            return flattened.Select(d =>
            {
                var ui = mapper.Map<UnitType, UnitInfo>(d.Key);
                ui.Count = d.Value;
                return ui;
            }).ToList();
        }
    }
}