using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Effects;
using StrategyGame.Model.Entities.Resources;
using StrategyGame.Model.Entities.Units;
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
                        if (total.ContainsKey(res.Child))
                        {
                            total[res.Child] += div.Count * res.MaintenanceAmount;
                        }
                        else
                        {
                            total.Add(res.Child, div.Count * res.MaintenanceAmount);
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
        /// <returns>The builder containing the modifiers for the country</returns>
        public static CountryModifierBuilder ParseAllEffect(this Country country, UnderSeaDatabaseContext context,
            GlobalValue globals, ModifierParserContainer Parsers)
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
                foreach (var e in country.CurrentEvent.Effects.Where(e => !e.Child.IsOneTime))
                {
                    if (!Parsers.TryParse(e.Child, country, context, builder, false))
                    {
                        Debug.WriteLine("Event effect with name {0} could not be handled by the provided parsers.",
                            e.Child.Name);
                    }
                }
            }

            // Then regular effects
            var effectparents = country.Buildings.Select(b => new
            {
                count = b.Amount,
                effects = b.Child.Effects
                    .Select(e => e.Child)
                    .Where(e => !e.IsOneTime)
            }).Concat(country.Researches.Select(r => new
            {
                count = r.Amount,
                effects = r.Child.Effects
                    .Select(e => e.Child)
                    .Where(e => !e.IsOneTime)
            })).ToList();

            foreach (var effectParent in effectparents)
            {
                for (int iii = 0; iii < effectParent.count; iii++)
                {
                    foreach (var e in effectParent.effects)
                    {
                        if (!Parsers.TryParse(e, country, context, builder, false))
                        {
                            Debug.WriteLine("Effect with name {0} could not be handled by the provided parsers.", e.Name);
                        }
                    }
                }
            }

            return builder;
        }

        /// <summary>
        /// Parses all effects of a country into a <see cref="CountryModifierBuilder"/>.
        /// </summary>
        /// <param name="country">The country to parse effects for. Its buildings, researches, events and their effects must be included.</param>
        /// <param name="effects">The collection of effects to apply.</param>
        /// <param name="context">The database to use.</param>
        /// <param name="Parsers">The collection of parsers to use.</param>
        public static void ApplyOneTime(this Country country, IEnumerable<Effect> effects,
            UnderSeaDatabaseContext context, ModifierParserContainer Parsers)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            var builder = new CountryModifierBuilder();

            foreach (var e in effects.Where(e => e.IsOneTime))
            {
                if (!Parsers.TryParse(e, country, context, builder, true))
                {
                    Debug.WriteLine("Event effect with name {0} could not be handled by the provided parsers.",
                        new[] { e.Name });
                }
            }
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
            var existing = target.Divisions.SingleOrDefault(d => d.Unit.Id == division.Unit.Id
                && d.BattleCount == division.BattleCount);

            if (existing == null)
            {
                division.ParentCommand = target;
                target.Divisions.Add(division);
            }
            else
            {
                existing.Count += division.Count;
                context.Divisions.Remove(existing);
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
            command.Divisions.MergeInto(target, context);
            context.Commands.Remove(command);
        }

        /// <summary>
        /// Merges the divisions into the provided command.
        /// </summary>
        /// <param name="divisions">The collection of <see cref="Division"/>s to merge.</param>
        /// <param name="target">The <see cref="Command"/> to merge into.</param>
        /// <param name="context">The database to use to remove the division if necessary.</param>
        public static void MergeInto(this IEnumerable<Division> divisions, Command target,
            UnderSeaDatabaseContext context)
        {
            foreach (var div in divisions)
            {
                div.MergeInto(target, context);
            }
        }

        /// <summary>
        /// Takes away a specified amount of units of a kind from a command.
        /// </summary>
        /// <param name="command">The command to take from.</param>
        /// <param name="unitId">The ID of the unit to take.</param>
        /// <param name="amount">The amount of units to take.</param>
        /// <param name="context">The database to use.</param>
        /// <returns>The <see cref="Divisions"/>s that were taken away.</returns>
        public static IEnumerable<Division> TakeFrom(this Command command, int unitId, int amount,
            UnderSeaDatabaseContext context)
        {
            var takenDivisions = new List<Division>();
            var fromDivisions = command.Divisions.Where(d => d.Unit.Id == unitId);
            if (fromDivisions.Sum(d => d.Count) < amount)
            {
                throw new ArgumentException("Not enough units");
            }

            foreach (var division in fromDivisions)
            {
                if (amount >= division.Count)
                {
                    division.ParentCommand = null;
                    takenDivisions.Add(division);
                    amount -= division.Count;
                }
                else
                {
                    division.Count -= amount;
                    var splitDivision = new Division
                    {
                        Unit = division.Unit,
                        BattleCount = division.BattleCount,
                        Count = amount
                    };

                    takenDivisions.Add(splitDivision);
                    context.Divisions.Add(splitDivision);
                    break;
                }
            }

            return takenDivisions;
        }

        /// <summary>
        /// Increases the battlecount of the units in the provided command. If the units reach the rank-up count, they are ranked up automatically
        /// </summary>
        /// <param name="command">The command to increase the battlecount for.</param>
        /// <param name="context">The database to use to remove the division if necessary.</param>
        public static void IncreaseBattleCount(this Command command, UnderSeaDatabaseContext context)
        {
            foreach (var div in command.Divisions.Where(d => !(d.Unit is SpyType)))
            {
                div.BattleCount++;

                if (div.Unit.CanRankUp && div.BattleCount >= div.Unit.BattlesToRankUp)
                {
                    var existing = command.Divisions.SingleOrDefault(d => d.BattleCount == 0 && d.Unit == div.Unit.RankedUpType);
                    div.Unit = div.Unit.RankedUpType;
                    div.BattleCount = 0;

                    if (existing != null)
                    {
                        div.MergeInto(command, context);
                    }
                }
            }
        }

        public static void Purchase<TEntity, TConnector>(this Country country,
            IPurchasable<TEntity, TConnector> purchasable, int count = 1)
            where TEntity : AbstractEntity<TEntity>
            where TConnector : AbstractConnectorWithAmount<TEntity, ResourceType>
        {
            foreach (var resource in purchasable.Cost)
            {
                var countryResource = country.Resources.SingleOrDefault(r => r.Child.Id == resource.Child.Id);

                if (countryResource == null || countryResource.Amount < resource.Amount * count)
                {
                    throw new ArgumentException("Not enough resource.");
                }

                countryResource.Amount -= resource.Amount * count;
            }
        }

        /// <summary>
        /// Adds a new country with its default data to the specified user.
        /// </summary>
        /// <param name="user">The user to add a country for.</param>
        /// <param name="name">The name of the country.</param>
        /// <param name="context">The database to use.</param>
        /// <returns>The task representing the state of the operation.</returns>
        public static async Task AddNewCountry(this User user, string name, UnderSeaDatabaseContext context)
        {
            var globals = await context.GlobalValues.SingleAsync();
            var newCountry = new Country()
            {
                Name = name,
                ParentUser = user,
                Resources = context.ResourceTypes.ToList()
                           .Select(r => new CountryResource { Amount = r.StartingAmount, Child = r }).ToList(),
                Score = -1,
                Rank = -1,
                CreatedRound = globals.Round
            };

            var defenders = new Command { ParentCountry = newCountry, TargetCountry = newCountry };

            context.Countries.Add(newCountry);
            context.Commands.Add(defenders);
            context.CountryBuildings.AddRange((await context.BuildingTypes.Where(b => b.IsStarting).ToListAsync())
                .Select(b => new AbstractConnectorWithAmount<Country, BuildingType> { Parent = newCountry, Amount = 1, Child = b }));
        }

        public static Dictionary<int, long> GetTotalProduction(this CountryModifierBuilder builder)
        {
            var result = new Dictionary<int, long>(builder.ResourceProductions.Count);
            foreach (var res in builder.ResourceProductions)
            {
                var mod = builder.ResourceModifiers.ContainsKey(res.Key)
                    ? builder.ResourceModifiers.Single(p => p.Key == res.Key).Value
                    : 1.0;

                result.Add(res.Key, (long)Math.Round(res.Value * mod));
            }
            return result;
        }
    }
}