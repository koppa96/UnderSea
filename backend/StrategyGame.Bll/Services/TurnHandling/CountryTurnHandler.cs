using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Resources;
using StrategyGame.Model.Entities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StrategyGame.Bll.Services.TurnHandling
{
    /// <summary>
    /// Handles turns in parts for individual countries.
    /// </summary>
    public class CountryTurnHandler
    {
        // Static random generation for attack power modification
        static readonly Random rng = new Random();

        protected ModifierParserContainer Parsers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryTurnHandler"/>.
        /// </summary>
        /// <param name="parsers">The <see cref="ModifierParserContainer"/> containing effect parsers used by the handler.</param>
        public CountryTurnHandler(ModifierParserContainer parsers)
        {
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        /// <summary>
        /// Handles pre-turn claculations, like building and research completitions, and income.
        /// Returns the <see cref="CountryModifierBuilder"/> containing the modifications for the country.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="country">The country to handle. The following must be included: Buildings, In progress buildings,
        /// researches, in progress researches, the buildings and researches of these, events, and the corresponding effects, commands, divisions and units.</param>
        /// <param name="globals">The <see cref="GlobalValue"/>s to use.</param>
        /// <param name="allEvents">The collection of all events that may occur.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public void HandlePreCombat(UnderSeaDatabaseContext context, Model.Entities.Country country,
            GlobalValue globals, IList<RandomEvent> allEvents)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (country == null)
            {
                throw new KeyNotFoundException();
            }

            if (globals == null)
            {
                throw new ArgumentNullException(nameof(globals));
            }

            // #1: Add a random event, and apply it if its permanent
            if (country.CreatedRound + globals.RandomEventGraceTimer <= globals.Round
                && rng.NextDouble() <= globals.RandomEventChance)
            {
                country.CurrentEvent = allEvents[rng.Next(allEvents.Count)];
                country.ApplyOneTime(country.CurrentEvent.Effects.Select(e => e.Child), context, Parsers);

                context.EventReports.Add(new EventReport
                {
                    Country = country,
                    Event = country.CurrentEvent,
                    IsSeen = false,
                    Round = globals.Round
                });
            }
            else if (country.CurrentEvent != null)
            {
                country.CurrentEvent = null;
            }

            var builder = country.ParseAllEffect(context, globals, Parsers);

            if (builder.WasEventIgnored)
            {
                country.CurrentEvent = null;
            }

            // #2: Resources
            foreach (var resource in builder.GetTotalProduction())
            {
                country.Resources.Single(r => r.Child.Id == resource.Key).Amount += resource.Value;
            }

            // #4: Pay soldiers
            DesertUnits(country);

            // #5: Add buildings that are completed
            CheckAddCompleted(country, context);
        }

        /// <summary>
        /// Handles combat calculations for all incoming attacks of a country. The order of the attacks is randomized.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="country">The country to handle. The commands, incoming attacks, the attack's divisions, parent country, 
        /// parent country builidings, researches, events, and effects and the parent user must be loaded.</param>
        /// <param name="globals">The <see cref="GlobalValue"/>s to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public void HandleCombat(UnderSeaDatabaseContext context, Model.Entities.Country country, GlobalValue globals)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (globals == null)
            {
                throw new ArgumentNullException(nameof(globals));
            }

            // Randomize the incoming attacks, excluding the defending forces and reinforcements, separate the reinforcements
            var incomingAttacks = country.IncomingAttacks
                .Where(c => !c.ParentCountry.Equals(country) && !c.ParentCountry.ParentUser.Equals(country.ParentUser))
                .Select(c => new { Order = rng.Next(), Command = c })
                .OrderBy(x => x.Order)
                .Select(x => x.Command)
                .ToList();

            var reinforcements = country.IncomingAttacks
                .Where(c => !c.ParentCountry.Equals(country) && c.ParentCountry.ParentUser.Equals(country.ParentUser))
                .ToList();

            var defenders = country.GetAllDefending();
            var builder = country.ParseAllEffect(context, globals, Parsers);

            foreach (var reinforcement in reinforcements)
            {
                reinforcement.MergeInto(defenders, context);
            }

            foreach (var attack in incomingAttacks)
            {
                var attackBuilder = attack.ParentCountry.ParseAllEffect(context, globals, Parsers);
                (double attackPower, double attackMods, double attackBase) = GetCurrentUnitPower(attack, globals,
                    true, attackBuilder);
                (double defensePower, double defenseMods, double defenseBase) = GetCurrentUnitPower(defenders, globals,
                    false, builder);

                var report = new CombatReport
                {
                    Attacker = attack.ParentCountry,
                    Defender = country,
                    Attackers = attack.Divisions.Select(d => new Division
                    { Count = d.Count, Unit = d.Unit }).ToList(),
                    Defenders = defenders.Divisions.Select(d => new Division
                    { Count = d.Count, Unit = d.Unit }).ToList(),
                    TotalAttackPower = attackPower,
                    TotalDefensePower = defensePower,
                    AttackModifier = attackMods,
                    DefenseModifier = defenseMods,
                    BaseAttackPower = attackBase,
                    BaseDefensePower = defenseBase,
                    Round = globals.Round,
                    Loot = new List<ReportResource>(0),
                    AttackerLosses = new List<Division>(0),
                    DefenderLosses = new List<Division>(0),
                    DefenderBuildings = country.Buildings.Select(b => new ConnectorWithAmount<CombatReport, BuildingType> { Amount = b.Amount, Child = b.Child }).ToList(),
                    DefenderResearches = country.Researches.Select(r => new ConnectorWithAmount<CombatReport, ResearchType> { Amount = r.Amount, Child = r.Child }).ToList(),
                    IsDeletedByAttacker = false,
                    IsDeletedByDefender = false,
                    IsSeenByAttacker = false,
                    IsSeenByDefender = false
                };

                var losses = attackPower > defensePower
                    ? CullUnits(defenders, globals.UnitLossOnLostBatle)
                    : CullUnits(attack, globals.UnitLossOnLostBatle);

                if (attackPower > defensePower)
                {
                    attack.IncreaseBattleCount(context);
                    var loots = CalculateLoot(country, globals.LootPercentage, attack, attackBuilder);

                    foreach (var resource in loots)
                    {
                        country.Resources.Single(r => r.Child == resource.Key).Amount -= resource.Value;
                    }

                    report.Loot = loots.Select(x => new ReportResource
                    {
                        Amount = x.Value,
                        RemainingAmount = country.Resources.Single(r => r.Child == x.Key).Amount,
                        Child = x.Key
                    }).ToList();
                    report.DefenderLosses = losses;
                }
                else
                {
                    report.AttackerLosses = losses;
                }

                CullSpies(attack, defenders, report.AttackerLosses, globals.SpyLossOnSuccess);
                context.Reports.Add(report);
            }
        }

        /// <summary>
        /// Handles post-combat calculations for a country, like calculating the score, and merging commands into the defense command.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="country">The country to handle. The commands, their divisions and units, buildings and researches, events, and their effects must be loaded.</param>
        /// <param name="globals">The <see cref="GlobalValue"/>s to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if an argument was null.</exception>
        public void HandlePostCombat(UnderSeaDatabaseContext context, Model.Entities.Country country, GlobalValue globals)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (globals == null)
            {
                throw new ArgumentNullException(nameof(globals));
            }

            var builder = country.ParseAllEffect(context, globals, Parsers);

            long divisionScore = 0;
            foreach (var comm in country.Commands)
            {
                divisionScore += comm.Divisions.Sum(d => d.Count);
            }

            country.Score = (long)Math.Round(
                builder.Population * globals.ScorePopulationMultiplier
                + country.Buildings.Count * globals.ScoreBuildingMultiplier
                + divisionScore * globals.ScoreUnitMultiplier
                + country.Researches.Count * globals.ScoreResearchMultiplier);

            // Merge all attacking commands into the defense command, delete attacking commands, and add the loot
            var defenders = country.GetAllDefending();

            foreach (var attack in country.Attacks.Where(x => x.DidAttackerWin && x.Round == globals.Round))
            {
                foreach (var res in attack.Loot)
                {
                    country.Resources.Single(r => r.Child == res.Child).Amount += res.Amount;
                }
            }

            foreach (var attack in country.Commands.Where(c => !c.Equals(defenders)).ToList())
            {
                attack.MergeInto(defenders, context);
            }
        }

        #region HelperMethods

        /// <summary>
        /// Calculate attack or defense power, based on the units in a command and their modifiers.
        /// Attack power is modified randomly by 5%.
        /// </summary>
        /// <param name="command">The command to get the combat power of.</param>
        /// <param name="globals">The global values.</param>
        /// <param name="doGetAttack">If attack or defense power should be calculated.</param>
        /// <param name="builder">The builder that contains the stat modifiers.</param>
        /// <returns>The combat power of the command.</returns>
        protected (double TotalPower, double Modifiers, double BasePower) GetCurrentUnitPower(Command command,
            GlobalValue globals, bool doGetAttack, CountryModifierBuilder builder)
        {
            if (doGetAttack)
            {
                var totalModifier = builder.AttackModifier *
                    ((rng.NextDouble() * globals.RandomAttackModifier) + 1 - (globals.RandomAttackModifier / 2));
                var basePower = command.Divisions.Sum(d => d.Count * (d.Unit.AttackPower + builder.AttackIncrease));
                return (basePower * totalModifier, totalModifier, basePower);
            }
            else
            {
                var basePower = command.Divisions.Sum(d => d.Count * d.Unit.DefensePower);
                return (basePower * builder.DefenseModifier, builder.DefenseModifier, basePower);
            }
        }

        /// <summary>
        /// Deletes some units from all divisions of a command. Divisions must be loaded.
        /// </summary>
        /// <param name="command">The command to cull.</param>
        /// <param name="lostPercentage">The percentage of units lost.</param>
        protected List<Division> CullUnits(Command command, double lostPercentage)
        {
            var totalLoss = (int)Math.Round(command.Divisions.Sum(d => d.Count) * lostPercentage);

            var losses = new Dictionary<UnitType, Division>();

            // TODO: Optimize
            if (totalLoss > 0)
            {
                while (totalLoss > 0)
                {
                    // To ensure proper distribution, the divisions are sorted into a dictionary based on their total counts
                    // then a random number is generated, and the first division that contain that unit position is selected.
                    int count = 0;
                    var lossable = command.Divisions.Where(d => d.Count > 0 && !(d.Unit is SpyType))
                        .ToDictionary(x => count += x.Count, x => x);
                    int target = rng.Next(lossable.Keys.Last());
                    var targetDiv = lossable.First(x => x.Key >= target).Value;

                    targetDiv.Count--;
                    totalLoss--;

                    if (losses.ContainsKey(targetDiv.Unit))
                    {
                        losses[targetDiv.Unit].Count++;
                    }
                    else
                    {
                        losses.Add(targetDiv.Unit, new Division { Count = 1, Unit = targetDiv.Unit });
                    }
                }
            }

            return losses.Values.ToList();
        }

        /// <summary>
        /// Culls spies for the attacker, according to the lost percentage.
        /// The numbers are floored, meaning there is always some spies lost.
        /// </summary>
        /// <param name="attack">The attacking command.</param>
        /// <param name="defense">The defending command.</param>
        /// <param name="existingLosses">The existing losses for the attacker.</param>
        /// <param name="lostPercentage">The percentage of spies lost.</param>
        protected void CullSpies(Command attack, Command defense, ICollection<Division> existingLosses, double lostPercentage)
        {
            var attacker = attack.Divisions.SingleOrDefault(x => x.Unit is SpyType);
            var defenderCount = defense.Divisions.SingleOrDefault(x => x.Unit is SpyType)?.Count ?? 0;

            if (attacker != null && attacker.Count != 0)
            {
                int loss = attacker.Count;
                if (attacker.Count > defenderCount)
                {
                    loss = attacker.Count - (int)Math.Floor(attacker.Count * (1 - lostPercentage));
                }

                attacker.Count -= loss;
                existingLosses.Add(new Division { Unit = attacker.Unit, Count = loss });
            }
        }

        /// <summary>
        /// Checks all in-progress buildings and researches of the country, 
        /// and adds any completed ones to it. Does not delete in progress values that are completed.
        /// This method does not perform any safety check regarding the amount of buildings or researches!
        /// </summary>
        /// <param name="country">The country to build in. The buildings, researches, 
        /// in progress buildings, researches, and their buildings and researches, must be included.</param>
        /// <param name="context">The database to use.</param>
        /// <returns>If the building could be started.</returns>
        protected void CheckAddCompleted(Model.Entities.Country country, UnderSeaDatabaseContext context)
        {
            foreach (var research in country.InProgressResearches
                .Where(r => r.TimeLeft == 0)
                .GroupBy(r => r.Child))
            {
                country.ApplyOneTime(Enumerable.Repeat(research.Key.Effects.Select(e => e.Child), research.Count())
                    .SelectMany(x => x), context, Parsers);
                var existing = country.Researches.FirstOrDefault(r => r.Child.Equals(research.Key));

                if (existing == null)
                {
                    var res = new ConnectorWithAmount<Model.Entities.Country, ResearchType>
                    {
                        Parent = country,
                        Child = research.Key,
                        Amount = research.Count()
                    };
                    country.Researches.Add(res);
                    context.CountryResearches.Add(res);
                }
                else
                {
                    existing.Amount += research.Count();
                }
            }

            foreach (var building in country.InProgressBuildings
                .Where(r => r.TimeLeft == 0)
                .GroupBy(b => b.Child))
            {
                country.ApplyOneTime(Enumerable.Repeat(building.Key.Effects.Select(e => e.Child), building.Count())
                    .SelectMany(x => x), context, Parsers);
                var existing = country.Buildings.FirstOrDefault(b => b.Child.Equals(building.Key));

                if (existing == null)
                {
                    var res = new ConnectorWithAmount<Model.Entities.Country, BuildingType>
                    {
                        Parent = country,
                        Child = building.Key,
                        Amount = building.Count()
                    };
                    country.Buildings.Add(res);
                    context.CountryBuildings.Add(res);
                }
                else
                {
                    existing.Amount += building.Count();
                }
            }
        }

        /// <summary>
        /// Deletes units until the supply / maintenance consumption can be satisfied by the country.
        /// Starts with the cheapest units. The commands, divisions and units must be included.
        /// </summary>
        /// <param name="country">The country to delete from.</param>
        protected void DesertUnits(Model.Entities.Country country)
        {
            var totalMaintenance = country.GetTotalMaintenance();

            if (totalMaintenance.Any(x => x.Value > country.Resources.Single(r => r.Child == x.Key).Amount))
            {
                foreach (var div in country.Commands.SelectMany(c => c.Divisions).OrderBy(d => d.Unit.Cost.First().Amount))
                {
                    var requiredReductions = div.Unit.Cost.ToDictionary(x => x.Child,
                        x =>
                        (long)Math.Ceiling(Math.Max(totalMaintenance[x.Child]
                        - country.Resources.Single(r => r.Child == x.Child).Amount, 0)
                        / (double)x.MaintenanceAmount));

                    int desertedAmount = (int)Math.Min(requiredReductions.Max(x => x.Value), div.Count);

                    div.Count -= desertedAmount;

                    foreach (var maint in div.Unit.Cost)
                    {
                        totalMaintenance[maint.Child] -= desertedAmount * maint.MaintenanceAmount;
                    }

                    if (totalMaintenance.All(x => x.Value > country.Resources.Single(r => r.Child == x.Key).Amount))
                    {
                        break;
                    }
                }
            }

            foreach (var maint in totalMaintenance)
            {
                country.Resources.Single(r => r.Child == maint.Key).Amount -= Math.Max(0, maint.Value);
            }
        }

        /// <summary>
        /// Calculates the loot based on the income of the attacker.
        /// </summary>
        /// <param name="country">The defending country.</param>
        /// <param name="lootPercentage">The maximal percentage of loot acquired.</param>
        /// <param name="attack">The attacking command.</param>
        /// <param name="builder">The modifier builder for the attacker.</param>
        /// <returns>The dictionary containing the loots.</returns>
        protected Dictionary<ResourceType, long> CalculateLoot(Model.Entities.Country country, double lootPercentage,
            Command attack, CountryModifierBuilder builder)
        {
            var productions = builder.GetTotalProduction();
            var maxLoot = country.Resources
                .OrderBy(r => productions.ContainsKey(r.Child.Id) ? productions[r.Child.Id] : 0)
                .ToDictionary(x => x.Child, x => (long)Math.Round(lootPercentage * x.Amount));
            var loots = new Dictionary<ResourceType, long>();
            var maxCarry = attack.Divisions.Sum(d => (long)d.Count * d.Unit.CarryCapacity);

            foreach (var loot in maxLoot)
            {
                var current = Math.Min(maxCarry, loot.Value);
                maxCarry -= current;
                loots.Add(loot.Key, current);

                if (maxCarry <= 0)
                {
                    break;
                }
            }

            return loots;
        }

        #endregion
    }
}