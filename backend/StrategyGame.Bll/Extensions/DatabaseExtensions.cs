using AutoMapper;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        /// Calculates the total maintenance of the units in the country.
        /// </summary>
        /// <param name="country">The <see cref="Country"/> to calculate total maintenance for.</param>
        /// <returns>The maintenance of all units in the country.</returns>
        public static Dictionary<ResourceType, long> GetTotalMaintenance(this Country country)
        {
            var total = new Dictionary<ResourceType, long>();
            foreach (var comm in country.Commands)
            {
                foreach (var div in comm.Divisions)
                {
                    foreach (var res in div.Unit.Cost)
                    {
                        if (total.ContainsKey(res.ResourceType))
                        {
                            total[res.ResourceType] += div.Count * res.MaintenanceAmount;
                        }
                        else
                        {
                            total.Add(res.ResourceType, div.Count * res.MaintenanceAmount);
                        }
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Parses all effects of a country into a <see cref="CountryModifierBuilder"/>.
        /// </summary>
        /// <param name="country">The country to parse effects for. Its buildings, researches, events and their effects must be included.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="globals">The <see cref="GlobalValue"/> to use.</param>
        /// <param name="Parsers">The collection of parsers to use.</param>
        /// <param name="doApplyEvent">If random events should be applied to the modifier.</param>
        /// <param name="doApplyPermanent">If effects that have permanenet effects should be applied.</param>
        /// <returns>The builder containing the modifiers for the country</returns>
        public static CountryModifierBuilder ParseAllEffectForCountry(this Country country, UnderSeaDatabaseContext context,
            GlobalValue globals, ModifierParserContainer Parsers, bool doApplyEvent, bool doApplyPermanent)
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
            if (doApplyEvent && country.CurrentEvent != null)
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
        /// Gets all <see cref="UnitInfo"/>s from all divisions of a collection of commands.
        /// The divisions, unit types and unit contents must be included in the country.
        /// </summary>
        /// <param name="commands">The collection of commands to get units for.</param>
        /// <param name="mapper">The mapper used to convert <see cref="UnitType"/> to <see cref="UnitInfo"/>.</param>
        /// <returns>The list of units.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static List<BriefUnitInfo> GetAllBriefUnitInfo(this IEnumerable<Command> commands, IMapper mapper)
        {
            var units = new Dictionary<int, BriefUnitInfo>();

            foreach (var command in commands)
            {
                var newInfos = command.GetAllBriefUnitInfo(mapper);

                foreach (var info in newInfos)
                {
                    if (units.ContainsKey(info.Id))
                    {
                        units[info.Id].TotalCount += info.TotalCount;
                        units[info.Id].DefendingCount += info.DefendingCount;
                    }
                    else
                    {
                        units.Add(info.Id, info);
                    }
                }
            }

            return units.Values.ToList();
        }

        /// <summary>
        /// Gets all <see cref="UnitInfo"/>s from all divisions of a command.
        /// The divisions, unit types and unit contents must be included in the country.
        /// </summary>
        /// <param name="command">The command to get units for.</param>
        /// <param name="mapper">The mapper used to convert <see cref="UnitType"/> to <see cref="UnitInfo"/>.</param>
        /// <returns>The list of units.</returns>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public static List<BriefUnitInfo> GetAllBriefUnitInfo(this Command command, IMapper mapper)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return command.Divisions.Select(x =>
            {
                var info = mapper.Map<Division, BriefUnitInfo>(x);

                if (command.ParentCountry.Equals(command.TargetCountry))
                {
                    info.DefendingCount = info.TotalCount;
                }

                return info;
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
                target.Divisions.Add(division);
                //division.ParentCommand = target;
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

        public static void Purchase<TEnity, TConnector>(this Country country, IPurchasable<TEnity, TConnector> purchasable, int count = 1)
            where TEnity : AbstractEntity<TEnity>
            where TConnector : AbstractResourceConnector<TEnity>
        {
            foreach (var resource in purchasable.Cost)
            {
                var countryResource = country.Resources.SingleOrDefault(r => r.ResourceType.Id == resource.ResourceType.Id);

                if (countryResource == null || countryResource.Amount < resource.Amount * count)
                {
                    throw new ArgumentException("Not enough resource.");
                }

                countryResource.Amount -= resource.Amount * count;
            }
        }
    }
}