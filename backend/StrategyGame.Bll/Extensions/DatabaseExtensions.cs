﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Extensions
{
    /// <summary>
    /// Provides exntension methods for the <see cref="Country"/> entity.
    /// </summary>
    public static class DatabaseExtensions
    {
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

        /// <summary>
        /// Parses all effects of a country into a <see cref="CountryModifierBuilder"/>.
        /// </summary>
        /// <param name="country">The country to parse effects for. Its buildings, researches and their effects must be included.</param>
        /// <param name="globals">The <see cref="GlobalValue"/> to use.</param>
        /// <param name="Parsers">The collection of parsers to use.</param>
        /// <returns></returns>
        public static CountryModifierBuilder ParseAllEffectForCountry(this Country country,
            GlobalValue globals, ModifierParserContainer Parsers)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            var effectparents = country.Buildings.Select(b => new
            {
                count = b.Count,
                effects = b.Building.Effects.Select(e => e.Effect)
            }).Concat(country.Researches.Select(r => new
            {
                count = r.Count,
                effects = r.Research.Effects.Select(e => e.Effect)
            })).ToList();

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

            foreach (var div in country.Commands.SelectMany(c => c.Divisions))
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