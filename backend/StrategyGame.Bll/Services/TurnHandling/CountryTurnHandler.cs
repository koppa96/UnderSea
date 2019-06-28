using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
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

            // #1: Add a random event
            if (country.CreatedRound + globals.RandomEventGraceTimer <= globals.Round
                && rng.NextDouble() <= globals.RandomEventChance)
            {
                country.CurrentEvent = allEvents[rng.Next(allEvents.Count)];
            }
            else if (country.CurrentEvent != null)
            {
                country.CurrentEvent = null;
            }

            // Apply permanent effects here
            var builder = country.ParseAllEffectForCountry(context, globals, Parsers, true, true);

            if (builder.WasEventIgnored)
            {
                country.CurrentEvent = null;
            }

            // #2: Tax
            country.Pearls += (long)Math.Round(builder.Population * globals.BaseTaxation * builder.TaxModifier
                + builder.PearlProduction);

            // #3: Coral (harvest)
            country.Corals += (long)Math.Round(builder.CoralProduction * builder.HarvestModifier);

            // #4: Pay soldiers
            DesertUnits(country);

            // #5: Add buildings that are completed
            CheckAddCompleted(country);
        }

        /// <summary>
        /// Handles combat calculations for all incoming attacks of a country. The order of the attacks is randomized.
        /// </summary>
        /// <param name="context">The <see cref="UnderSeaDatabaseContext"/> that is used to access the database.</param>
        /// <param name="country">The country to handle. The commands, incoming attacks, the attack's divisions, parent country, 
        /// parent country builidings, researches, events, and effects must be loaded.</param>
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

            // Randomize the incoming attacks, excluding the defending forces
            var incomingAttacks = country.IncomingAttacks
                .Where(c => !c.ParentCountry.Equals(country))
                .Select(c => new { Order = rng.Next(), Command = c })
                .OrderBy(x => x.Order)
                .Select(x => x.Command)
                .ToList();

            var defenders = country.GetAllDefending();
            var builder = country.ParseAllEffectForCountry(context, globals, Parsers, true, false);

            foreach (var attack in incomingAttacks)
            {
                (double attackPower, double attackMods, double attackBase) = GetCurrentUnitPower(attack, globals, true,
                    attack.ParentCountry.ParseAllEffectForCountry(context, globals, Parsers, true, false));
                (double defensePower, double defenseMods, double defenseBase) = GetCurrentUnitPower(defenders, globals,
                    false, builder);

                var losses = attackPower > defensePower
                    ? CullUnits(defenders, globals.UnitLossOnLostBatle)
                    : CullUnits(attack, globals.UnitLossOnLostBatle);

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
                    PearlLoot = 0,
                    CoralLoot = 0,
                    Losses = losses
                };

                if (attackPower > defensePower)
                {
                    var pearlLoot = (long)Math.Round(country.Pearls * globals.LootPercentage);
                    var coralLoot = (long)Math.Round(country.Corals * globals.LootPercentage);
                    country.Pearls -= pearlLoot;
                    country.Corals -= coralLoot;
                    report.CoralLoot = coralLoot;
                    report.PearlLoot = pearlLoot;
                }

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

            var builder = country.ParseAllEffectForCountry(context, globals, Parsers, true, false);

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
                country.Pearls += attack.PearlLoot;
                country.Corals += attack.CoralLoot;
            }

            foreach (var attack in country.Commands.Where(c => c.Id != defenders.Id).ToList())
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

            var losses = new Dictionary<int, Division>();

            // TODO: Optimize
            if (totalLoss > 0)
            {
                while (totalLoss > 0)
                {
                    // To ensure proper distribution, the divisions are sorted into a dictionary based on their total counts
                    // then a random number is generated, and the first division that contain that unit position is selected.
                    int count = 0;
                    var lossable = command.Divisions.Where(d => d.Count > 0).ToDictionary(x => count += x.Count, x => x);
                    int target = rng.Next(lossable.Keys.Last());
                    var targetDiv = lossable.First(x => x.Key >= target).Value;

                    targetDiv.Count--;
                    totalLoss--;

                    if (losses.ContainsKey(targetDiv.Unit.Id))
                    {
                        losses[targetDiv.Unit.Id].Count++;
                    }
                    else
                    {
                        losses.Add(targetDiv.Unit.Id, new Division { Count = 1, Unit = targetDiv.Unit });
                    }
                }
            }

            return losses.Values.ToList();
        }

        /// <summary>
        /// Checks all in-progress buildings and researches of the country, 
        /// and adds any completed ones to it. Does not delete in progress values that are completed.
        /// This method does not perform any safety check regarding the amount of buildings or researches!
        /// </summary>
        /// <param name="country">The country to build in. The buildings, researches, 
        /// in progress buildings, researches, and their buildings and researches, must be included.</param>
        /// <returns>If the building could be started.</returns>
        protected void CheckAddCompleted(Model.Entities.Country country)
        {
            var researches = country.InProgressResearches
                .Where(r => r.TimeLeft == 0)
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
                        country.Researches.Add(new CountryResearch
                        {
                            Research = research.Key,
                            Count = research.Count()
                        });
                    }
                    else
                    {
                        existing.Count += research.Count();
                    }
                }
            }

            // Get and complete buildings
            var buildings = country.InProgressBuildings
                .Where(r => r.TimeLeft == 0)
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
                        country.Buildings.Add(new CountryBuilding()
                        {
                            Building = building.Key,
                            Count = building.Count()
                        });
                    }
                    else
                    {
                        existing.Count += building.Count();
                    }
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
            var (pearlUpkeep, coralUpkeep) = country.GetTotalMaintenance();

            if (coralUpkeep > country.Corals || pearlUpkeep > country.Pearls)
            {
                long pearlDeficit = Math.Max(pearlUpkeep - country.Pearls, 0);
                long coralDeficit = Math.Max(coralUpkeep - country.Corals, 0);

                foreach (var div in country.Commands.SelectMany(c => c.Divisions).OrderBy(d => d.Unit.CostPearl))
                {
                    long requiredPearlReduction = (long)Math.Ceiling((double)pearlDeficit / div.Unit.MaintenancePearl);
                    long requiredCoralReduction = (long)Math.Ceiling((double)coralDeficit / div.Unit.MaintenanceCoral);

                    int desertedAmount = (int)Math.Min(Math.Max(requiredCoralReduction, requiredPearlReduction), div.Count);

                    div.Count -= desertedAmount;

                    pearlDeficit -= desertedAmount * div.Unit.MaintenancePearl;
                    pearlUpkeep -= desertedAmount * div.Unit.MaintenancePearl;
                    coralDeficit -= desertedAmount * div.Unit.MaintenanceCoral;
                    coralUpkeep -= desertedAmount * div.Unit.MaintenanceCoral;

                    if (coralUpkeep <= country.Corals && pearlUpkeep <= country.Pearls)
                    {
                        break;
                    }
                }
            }

            pearlUpkeep = Math.Max(0, pearlUpkeep);
            coralUpkeep = Math.Max(0, coralUpkeep);

            country.Pearls -= pearlUpkeep;
            country.Corals -= coralUpkeep;
        }

        #endregion
    }
}