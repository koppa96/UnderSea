using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
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
        /// <param name="country">The country to parse effects for. Its buildings, researches, events and their effects must be included.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="globals">The <see cref="GlobalValue"/> to use.</param>
        /// <param name="Parsers">The collection of parsers to use.</param>
        /// <param name="doApplyPermanent">If effects that have permanenet effects should be applied.</param>
        /// <returns>The builder containing the modifiers for the country</returns>
        public static CountryModifierBuilder ParseAllEffectForCountry(this Country country, UnderSeaDatabaseContext context,
            GlobalValue globals, ModifierParserContainer Parsers, bool doApplyPermanent = false)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            // Set up builder
            var builder = new CountryModifierBuilder
            {
                BarrackSpace = globals.StartingBarrackSpace,
                Population = globals.StartingPopulation
            };

            // First handle events
            if (country.CurrentEvent != null)
            {
                foreach (var e in country.CurrentEvent.Effects)
                {
                    if (!Parsers.TryParse(e.Effect, country, context, builder, doApplyPermanent))
                    {
                        Debug.WriteLine("Event effect with name {0} could not be handled by the provided parsers.", e.Effect.Name);
                    }
                }
            }

            // Then regular effects
            var effectparents = country.Buildings.Select(b => new
            {
                count = b.Count,
                effects = b.Building.Effects.Select(e => e.Effect)
            }).Concat(country.Researches.Select(r => new
            {
                count = r.Count,
                effects = r.Research.Effects.Select(e => e.Effect)
            })).ToList();

            foreach (var effectParent in effectparents)
            {
                for (int iii = 0; iii < effectParent.count; iii++)
                {
                    foreach (var e in effectParent.effects)
                    {
                        if (!Parsers.TryParse(e, country, context, builder, doApplyPermanent))
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
        public static async Task<IEnumerable<BriefUnitInfo>> GetAllBriefUnitInfoAsync(this Country country, UnderSeaDatabaseContext context,
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
                var ui = mapper.Map<UnitType, BriefUnitInfo>(d.Key);
                ui.Count = d.Value;
                return ui;
            }).ToList();
        }

        /// <summary>
        /// Merges the division into the provided command.
        /// </summary>
        /// <param name="division">The <see cref="Division"/> to merge.</param>
        /// <param name="target">The <see cref="Command"/> to merge into.</param>
        /// <param name="context">The database to use to remove the division if necessary.</param>
        public static void MergeInto(this Division division, Command target, UnderSeaDatabaseContext context)
        {
            var existing = target.Divisions.SingleOrDefault(d => d.Unit.Id == division.Unit.Id);

            if (existing == null)
            {
                division.ParentCommand = target;
            }
            else
            {
                existing.Count += division.Count;
                context.Divisions.Remove(division);
            }
        }

        /// <summary>
        /// Merges the command into the provided command.
        /// </summary>
        /// <param name="command">The <see cref="Command"/> to merge.</param>
        /// <param name="target">The <see cref="Command"/> to merge into.</param>
        /// <param name="context">The database to use to remove the division if necessary.</param>
        public static void MergeInto(this Command command, Command target, UnderSeaDatabaseContext context)
        {
            foreach (var div in command.Divisions)
            {
                div.MergeInto(target, context);
            }

            context.Commands.Remove(command);
        }
    }
}