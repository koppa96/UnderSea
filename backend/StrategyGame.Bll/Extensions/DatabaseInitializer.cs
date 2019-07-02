using Microsoft.EntityFrameworkCore;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Extensions
{
    /// <summary>
    /// Provides helper emthods to fill the database with values from the specification.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Removes all data from the database.
        /// </summary>
        /// <param name="context">The dbcontext to use.</param>
        /// <returns>The task representing the operation.</returns>
        public static Task PurgeDatabaseAsync(this UnderSeaDatabaseContext context)
        {
            // Purge connecting tables
            context.BuildingEffects.RemoveRange(context.BuildingEffects);
            context.ResearchEffects.RemoveRange(context.ResearchEffects);
            context.InProgressBuildings.RemoveRange(context.InProgressBuildings);
            context.InProgressResearches.RemoveRange(context.InProgressResearches);
            context.CountryBuildings.RemoveRange(context.CountryBuildings);
            context.CountryResearches.RemoveRange(context.CountryResearches);
            context.EventEffects.RemoveRange(context.EventEffects);

            // Purge contents
            context.BuildingContents.RemoveRange(context.BuildingContents);
            context.ResearchContents.RemoveRange(context.ResearchContents);
            context.UnitContents.RemoveRange(context.UnitContents);
            context.EventContents.RemoveRange(context.EventContents);

            // Purge commands, divisions, and units
            context.Reports.RemoveRange(context.Reports);
            context.Commands.RemoveRange(context.Commands);
            context.Divisions.RemoveRange(context.Divisions);
            context.UnitTypes.RemoveRange(context.UnitTypes);

            // Purge "static" values
            context.BuildingTypes.RemoveRange(context.BuildingTypes);
            context.Countries.RemoveRange(context.Countries);
            context.Effects.RemoveRange(context.Effects);
            context.GlobalValues.RemoveRange(context.GlobalValues);
            context.ResearchTypes.RemoveRange(context.ResearchTypes);
            context.RandomEvents.RemoveRange(context.RandomEvents);

            // Purge users
            context.Users.RemoveRange(context.Users);
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// Fills the database with default data.
        /// </summary>
        /// <param name="context">The dbcontext to use.</param>
        /// <returns>The task representing the operation.</returns>
        public static async Task FillWithDefaultAsync(this UnderSeaDatabaseContext context)
        {
            // Add contents
            var currentCont = new BuildingContent
            {
                Name = "Áramlásirányító",
                Description = "+50 lakos, 200 korall / kör",
                ImageUrl = "images/static/buildings/aramlasiranyito-lg.png",
                IconImageUrl = "images/static/buildings/aramlasiranyito-icon.svg"
            };
            var reefCastCont = new BuildingContent
            {
                Name = "Zátonyvár",
                Description = "+200 szállás",
                ImageUrl = "images/static/buildings/zatonyvar-lg.png",
                IconImageUrl = "images/static/buildings/zatonyvar-icon.svg"
            };
            var stonemineCont = new BuildingContent
            {
                Name = "Kőbánya",
                Description = "+200 kő körönként",
            };
            context.BuildingContents.AddRange(currentCont, reefCastCont, stonemineCont);

            var sealCont = new UnitContent
            {
                Name = "Rohamfóka",
                Description = "Jól támad de rosszul véd",
                ImageUrl = "images/static/units/rohamfoka.svg",
                IconImageUrl = "images/static/units/rohamfoka.svg"
            };
            var ponyCont = new UnitContent
            {
                Name = "Csatacsikó",
                Description = "Jól véd de rosszul támad",
                ImageUrl = "images/static/units/csatacsiko.svg",
                IconImageUrl = "images/static/units/csatacsiko.svg"
            };
            var lazorCont = new UnitContent
            {
                Name = "Lézercápa",
                Description = "lazers man",
                ImageUrl = "images/static/units/lezercapa.svg",
                IconImageUrl = "images/static/units/lezercapa.svg"
            };
            var leaderCont = new UnitContent
            {
                Name = "Parancsnok",
                Description = "Támadást csak parancsnok tud vezetni",
            };

            context.UnitContents.AddRange(sealCont, ponyCont, lazorCont, leaderCont);

            var mudTCont = new ResearchContent
            {
                Name = "Iszap traktor",
                Description = "Iszapozza a korallt (amitől amúgy IRL meghalna, korall nem növény nem kell neki föld), +10% korall termelés",
                ImageUrl = "images/static/researches/iszaptraktor-lg.png",
                IconImageUrl = "images/static/researches/iszaptraktor-sm.png"
            };
            var mudCCont = new ResearchContent
            {
                Name = "Iszap kombájn",
                Description = "Nagyon iszapozza a korallt, +15% korall termelés",
                ImageUrl = "images/static/researches/iszapkombajn-lg.png",
                IconImageUrl = "images/static/researches/iszapkombajn-sm.png"
            };
            var defCont = new ResearchContent
            {
                Name = "Korallfal",
                Description = "Fal, korallból. +20% védekezés",
                ImageUrl = "images/static/researches/korallfal.svg",
                IconImageUrl = "images/static/researches/korallfal.svg"
            };
            var attCont = new ResearchContent
            {
                Name = "Szonárágyú",
                Description = "Mint a denevér, echo-lokáció. +20% támadás",
                ImageUrl = "images/static/researches/szonaragyu-lg.png",
                IconImageUrl = "images/static/researches/szonaragyu-sm.png"
            };
            var cCont = new ResearchContent
            {
                Name = "Vízalatti harcművészetek",
                Description = "\"A különbség a lehetetlen és a lehetséges között az egyén akarata.\", +10% védekezés és támadás",
                ImageUrl = "images/static/researches/vizalatti-harcmuveszetek.svg",
                IconImageUrl = "images/static/researches/vizalatti-harcmuveszetek.svg"
            };
            var taxCont = new ResearchContent
            {
                Name = "Alkímia",
                Description = "A népesség pénzt csinál, +30% adó bevétel",
                ImageUrl = "images/static/researches/alkimia.svg",
                IconImageUrl = "images/static/researches/alkimia.svg"
            };

            context.ResearchContents.AddRange(mudTCont, mudCCont, defCont, attCont, cCont, taxCont);

            var plagueCont = new EventContent
            {
                Name = "Pestis",
                Description = "Az országodban kitört a pestis, elveszítesz 50 embert és egy áramlásirányítót.",
                FlavourText = "Hozzátok a halottakat!"
            };
            var fireCont = new EventContent
            {
                Name = "Víz alatti tűz",
                Description = "Az országodban tűz ütött ki és leégett egy zátonyvár.",
                FlavourText = "Tűz víz alatt? Micsoda?!"
            };
            var mineCont = new EventContent
            {
                Name = "Aranybánya",
                Description = "Az embereid felfedeztek egy új aranybányát, kapsz 1000 bónusz aranyat.",
                FlavourText = "Nagyon fényes"
            };
            var goodhvCont = new EventContent
            {
                Name = "Jó termés",
                Description = "Minden áramlásirányító +50 korallt ad ebben a körben.",
                FlavourText = "A termés egy stabil ország alapja"
            };
            var badhvCont = new EventContent
            {
                Name = "Rossz termés",
                Description = "Minden áramlásirányító -50 korallt ad ebben a körben.",
                FlavourText = "A király lakomázik, a paraszt éhezik"
            };
            var contPopCont = new EventContent
            {
                Name = "Elégedett emberek",
                Description = "Az országodban elégedettek az emberek, ezért extra 50 ember költözött be és építettek maguknak egy áramlásirányítót.",
                FlavourText = "Nő a nép, nő a felelősség"
            };
            var discontPopCont = new EventContent
            {
                Name = "Elégedetlen emberek",
                Description = "Az országodban elégedetlenek az emberek, ezért 50 ember elköltözött és az áramlásirányítójukat lerombolták.",
                FlavourText = "A paraszt elmegy, pusztítást hagy maga után"
            };
            var contSolCont = new EventContent
            {
                Name = "Elégedett katonák",
                Description = "Katonáid elégedettek ebben a körben, minden katona támadása nő eggyel.",
                FlavourText = "Elégedett katona motivált katona"
            };
            var disconSolCont = new EventContent
            {
                Name = "Elégedetlen katonák",
                Description = "Katonáid elégedetlenek ebben a körben, minden katona támadása csökken eggyel.",
                FlavourText = "Elsőnek a morál, utána a hűség"
            };

            context.EventContents.AddRange(plagueCont, mineCont, fireCont, goodhvCont, badhvCont,
                contPopCont, contSolCont, disconSolCont, discontPopCont);

            var coralCont = new ResourceContent
            {
                Name = "Korall",
                Description = "Kis rákok várat építenek",
            };
            var pearlCont = new ResourceContent
            {
                Name = "Gyöngy",
                Description = "gyöngy",
            };
            var stoneCont = new ResourceContent
            {
                Name = "Kő",
                Description = "Kő, kavics",
            };
            context.ResourceContents.AddRange(coralCont, pearlCont, stoneCont);
            await context.SaveChangesAsync();

            // Resources
            var coral = new ResourceType { Content = coralCont, StartingAmount = 500, NewCountryCost = 100000 };
            var pearl = new ResourceType { Content = coralCont, StartingAmount = 1000, NewCountryCost = 100000 };
            var stone = new ResourceType { Content = coralCont, StartingAmount = 1000, NewCountryCost = 100000 };
            context.ResourceTypes.AddRange(coral, pearl, stone);
            await context.SaveChangesAsync();

            // Effects, Buildings, researches
            // áramlásirányító
            var popIn = new Effect
            {
                Name = KnownValues.PopulationIncrease,
                Value = 50,
                Parameter = pearl.Id.ToString() + ":" + 25
            };
            var cp = new Effect { Name = KnownValues.ResourceProductionIncrease, Value = 200, Parameter = coral.Id.ToString() };
            var currentController = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, ResourceType = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = currentCont
            };

            // zátonyvár
            var bsIn = new Effect { Name = KnownValues.BarrackSpaceIncrease, Value = 200 };
            var reefCastle = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, ResourceType = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = reefCastCont
            };

            // kőbánya
            var stIn = new Effect { Name = KnownValues.ResourceProductionIncrease, Value = 200, Parameter = stone.Id.ToString() };
            var stoneMine = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, ResourceType = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = reefCastCont
            };

            // Iszaptraktor
            var harvMod1 = new Effect { Name = KnownValues.HarvestModifier, Value = 0.1 };
            var mudT = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = mudTCont };

            // Iszapkombájn
            var harvMod2 = new Effect { Name = KnownValues.HarvestModifier, Value = 0.15 };
            var mudC = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = mudCCont };

            // korallfal
            var defMod1 = new Effect { Name = KnownValues.UnitDefenseModifier, Value = 0.2 };
            var wall = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = defCont };

            // Szonárágyú
            var attMod1 = new Effect { Name = KnownValues.UnitAttackModifier, Value = 0.2 };
            var canon = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = attCont };

            // Harcművészet
            var combModA = new Effect { Name = KnownValues.UnitAttackModifier, Value = 0.1 };
            var combModD = new Effect { Name = KnownValues.UnitDefenseModifier, Value = 0.1 };
            var martialArts = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = cCont };

            // Alchemy
            var taxMod1 = new Effect { Name = KnownValues.TaxationModifier, Value = 0.3 };
            var alchemy = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, ResourceType = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = taxCont };


            // Add effects, buildings, researches
            context.Effects.AddRange(popIn, cp, bsIn, harvMod1, harvMod2,
                defMod1, attMod1, combModA, combModD, taxMod1, stIn);
            context.BuildingTypes.AddRange(currentController, reefCastle, stoneMine);
            context.ResearchTypes.AddRange(mudT, mudC, wall, canon, martialArts, alchemy);
            await context.SaveChangesAsync();


            // Add effects to buildings and researches
            context.BuildingEffects.AddRange(
                new BuildingEffect { Building = currentController, Effect = popIn },
                new BuildingEffect { Building = currentController, Effect = cp },
                new BuildingEffect { Building = reefCastle, Effect = bsIn });

            context.ResearchEffects.AddRange(
                new ResearchEffect { Research = mudT, Effect = harvMod1 },
                new ResearchEffect { Research = mudC, Effect = harvMod2 },
                new ResearchEffect { Research = wall, Effect = defMod1 },
                new ResearchEffect { Research = canon, Effect = attMod1 },
                new ResearchEffect { Research = martialArts, Effect = combModA },
                new ResearchEffect { Research = martialArts, Effect = combModD },
                new ResearchEffect { Research = alchemy, Effect = taxMod1 });
            await context.SaveChangesAsync();

            // Add units
            // rohamfóka
            var seal3 = new UnitType
            {
                AttackPower = 10,
                DefensePower = 5,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                IsPurchasable = false,
                Content = sealCont
            };
            var seal2 = new UnitType
            {
                AttackPower = 8,
                DefensePower = 3,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                BattlesToLevelUp = 5,
                RankedUpType = seal3,
                IsPurchasable = false,
                Content = sealCont
            };
            var seal = new UnitType
            {
                AttackPower = 6,
                DefensePower = 2,
                Cost = new[]
                {
                    new UnitResource { Amount = 50, MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                BattlesToLevelUp = 3,
                RankedUpType = seal2,
                IsPurchasable = true,
                Content = sealCont
            };
            // csatacsikó
            var pony3 = new UnitType
            {
                AttackPower = 5,
                DefensePower = 10,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                IsPurchasable = false,
                Content = ponyCont
            };
            var pony2 = new UnitType
            {
                AttackPower = 3,
                DefensePower = 8,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                BattlesToLevelUp = 5,
                RankedUpType = pony3,
                IsPurchasable = false,
                Content = ponyCont
            };
            var pony = new UnitType
            {
                AttackPower = 2,
                DefensePower = 6,
                Cost = new[]
                {
                    new UnitResource {Amount = 50, MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { Amount = 1, ResourceType = coral }
                },
                BattlesToLevelUp = 3,
                RankedUpType = pony2,
                IsPurchasable = true,
                Content = ponyCont
            };
            // lézercápa
            var lazor3 = new UnitType
            {
                AttackPower = 10,
                DefensePower = 10,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 3, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 2, ResourceType = coral }
                },
                IsPurchasable = false,
                Content = lazorCont
            };
            var lazor2 = new UnitType
            {
                AttackPower = 7,
                DefensePower = 7,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 1, ResourceType = coral }
                },
                BattlesToLevelUp = 5,
                RankedUpType = lazor3,
                IsPurchasable = false,
                Content = lazorCont
            };
            var lazor = new UnitType
            {
                AttackPower = 5,
                DefensePower = 5,
                Cost = new[]
                {
                    new UnitResource {Amount = 100, MaintenanceAmount = 3, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 2, ResourceType = coral }
                },
                BattlesToLevelUp = 3,
                RankedUpType = lazor2,
                IsPurchasable = true,
                Content = lazorCont
            };
            // hadvezér
            var leader = new LeaderType
            {
                AttackPower = 0,
                DefensePower = 0,
                Cost = new[]
                {
                    new UnitResource {Amount = 400, MaintenanceAmount = 4, ResourceType = pearl },
                    new UnitResource { MaintenanceAmount = 2, ResourceType = coral }
                },
                BattlesToLevelUp = 3,
                IsPurchasable = true,
                Content = leaderCont
            };

            context.UnitTypes.AddRange(seal3, seal2, seal, pony3, pony2, pony, lazor3, lazor2, lazor, leader);
            await context.SaveChangesAsync();


            // Add events
            var plague = new RandomEvent { Content = plagueCont };
            var removeCurrent = new Effect
            { Parameter = currentController.Id.ToString(), Name = KnownValues.AddBuildingEffect, Value = -1 };

            var fire = new RandomEvent { Content = fireCont };
            var removeCastle = new Effect
            { Parameter = reefCastle.Id.ToString(), Name = KnownValues.AddBuildingEffect, Value = -1 };

            var mine = new RandomEvent { Content = mineCont };
            var addPearl = new Effect
            { Value = 1000, Name = KnownValues.ResourceProductionIncrease, Parameter = pearl.Id.ToString() };

            var goodHarvest = new RandomEvent { Content = goodhvCont };
            var extraCoral = new Effect
            {
                Name = KnownValues.BuildingProductionIncrease,
                Value = 50,
                Parameter = currentController.Id.ToString() + ";" + coral.Id.ToString()
            };

            var badHarvest = new RandomEvent { Content = badhvCont };
            var lessCoral = new Effect
            { Name = KnownValues.BuildingProductionIncrease, Value = -50 };

            var contentPopulation = new RandomEvent { Content = contPopCont };
            var addCurrent = new Effect
            { Name = KnownValues.AddBuildingEffect, Parameter = currentController.Id.ToString(), Value = 1 };
            var discontentPopulation = new RandomEvent();

            var contentSoldiers = new RandomEvent { Content = contSolCont };
            var addAttack = new Effect
            { Name = KnownValues.IncreaseUnitAttack, Value = 1 };

            var discontentSoldiers = new RandomEvent { Content = disconSolCont };
            var removeAttack = new Effect
            { Name = KnownValues.IncreaseUnitAttack, Value = -1 };

            await context.SaveChangesAsync();

            // Add event effects
            context.EventEffects.AddRange(
                new EventEffect { Effect = removeCurrent, Event = plague },
                new EventEffect { Effect = removeCastle, Event = fire },
                new EventEffect { Effect = addPearl, Event = mine },
                new EventEffect { Effect = extraCoral, Event = goodHarvest },
                new EventEffect { Effect = lessCoral, Event = badHarvest },
                new EventEffect { Effect = addCurrent, Event = contentPopulation },
                new EventEffect { Effect = removeCurrent, Event = discontentPopulation },
                new EventEffect { Effect = addAttack, Event = contentSoldiers },
                new EventEffect { Effect = removeAttack, Event = discontentSoldiers }
            );
            await context.SaveChangesAsync();

            // globals
            context.GlobalValues.Add(new GlobalValue
            {
                Round = 1,
                StartingBarrackSpace = 0,
                StartingPopulation = 0,
                LootPercentage = 0.5,
                UnitLossOnLostBatle = 0.1,
                RandomEventChance = 0.1,
                RandomEventGraceTimer = 10,
                ScoreBuildingMultiplier = 50,
                ScoreResearchMultiplier = 100,
                ScorePopulationMultiplier = 1,
                ScoreUnitMultiplier = 5,
                FirstStartingBuilding = reefCastle,
                SecondStartingBuilding = currentController,
                RandomAttackModifier = 0.1
            });
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Fills the database with some test users, armies, buildings.
        /// </summary>
        /// <param name="context">The dbcontext to use.</param>
        /// <returns>The task representing the operation.</returns>
        public static async Task AddTestUsersAsync(this UnderSeaDatabaseContext context)
        {
            var thePoor = new User
            { UserName = "ThePoor", Email = "poor@test.com" };
            var theRich = new User
            { UserName = "TheRich", Email = "rich@test.com" };
            var theCommander = new User
            { UserName = "TheCommander", Email = "comm@test.com" };
            var theBuilder = new User
            { UserName = "TheBuilder", Email = "build@test.com" };
            var theResearcher = new User
            { UserName = "TheResearcher", Email = "res@test.com" };
            context.AddRange(thePoor, theRich, theCommander, theBuilder, theResearcher);

            var pc = new Country
            {
                Name = "poor",
                ParentUser = thePoor,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = 0, ResourceType = x }).ToList()
            };
            var rc = new Country
            {
                Name = "rich",
                ParentUser = theRich,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = 100000, ResourceType = x }).ToList()
            };
            var cc = new Country
            {
                Name = "attacky",
                ParentUser = theCommander,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, ResourceType = x }).ToList()
            };
            var bc = new Country
            {
                Name = "poi",
                ParentUser = theBuilder,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, ResourceType = x }).ToList()
            };
            var sc = new Country
            {
                Name = "science",
                ParentUser = theResearcher,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, ResourceType = x }).ToList()
            };
            context.Countries.AddRange(pc, rc, cc, bc, sc);

            var d1 = new Command { ParentCountry = pc, TargetCountry = pc };
            var d2 = new Command { ParentCountry = rc, TargetCountry = rc };
            var d3 = new Command { ParentCountry = cc, TargetCountry = cc };
            var d4 = new Command { ParentCountry = bc, TargetCountry = bc };
            var d5 = new Command { ParentCountry = sc, TargetCountry = sc };
            var d6 = new Command { ParentCountry = pc, TargetCountry = cc };
            context.Commands.AddRange(d1, d2, d3, d4, d5, d6);

            var b1 = await context.BuildingTypes.FirstAsync();
            var b2 = await context.BuildingTypes.Skip(1).FirstAsync();

            context.CountryBuildings.AddRange(new CountryBuilding
            {
                Building = b1,
                Count = 1,
                ParentCountry = bc
            }, new CountryBuilding
            {
                Building = b2,
                Count = 13,
                ParentCountry = bc
            });

            context.InProgressBuildings.Add(new InProgressBuilding
            { Building = context.BuildingTypes.First(), ParentCountry = cc, TimeLeft = 1 });
            context.InProgressResearches.Add(new InProgressResearch
            { Research = context.ResearchTypes.First(), ParentCountry = cc, TimeLeft = 1 });

            var r1 = await context.ResearchTypes.FirstAsync();
            var r2 = await context.ResearchTypes.Skip(3).FirstAsync();

            context.CountryResearches.AddRange(new CountryResearch
            {
                Research = r1,
                Count = 1,
                ParentCountry = sc
            }, new CountryResearch
            {
                Research = r2,
                Count = 1,
                ParentCountry = sc
            });

            var u1 = await context.UnitTypes.FirstAsync();
            var u2 = await context.UnitTypes.Skip(1).FirstAsync();
            var u3 = await context.UnitTypes.Skip(2).FirstAsync();
            var leader = await context.UnitTypes.FirstAsync(u => u is LeaderType);

            context.Divisions.AddRange(new Division
            {
                Count = 50,
                ParentCommand = d3,
                Unit = u1
            }, new Division
            {
                Count = 10,
                ParentCommand = d3,
                Unit = u2
            }, new Division
            {
                Count = 1,
                ParentCommand = d3,
                Unit = u3
            }, new Division
            {
                Count = 500,
                ParentCommand = d6,
                Unit = u3
            }, new Division
            {
                Count = 1,
                ParentCommand = d3,
                Unit = leader
            });

            var attackers = new Division
            {
                Count = 20,
                Unit = await context.UnitTypes.FirstAsync()
            };

            var defenders = new Division
            {
                Count = 10,
                Unit = await context.UnitTypes.FirstAsync()
            };

            var losses = new Division
            {
                Count = 1,
                Unit = await context.UnitTypes.FirstAsync()
            };

            context.Reports.Add(new CombatReport
            {
                Attacker = sc,
                Defender = bc,
                Attackers = new[] { attackers },
                Defenders = new[] { defenders },
                Losses = new[] { losses },
                AttackModifier = 1.1,
                DefenseModifier = 1,
                BaseAttackPower = 20,
                BaseDefensePower = 10,
                TotalAttackPower = 1337,
                TotalDefensePower = 60,
                Loot = new[]
                {
                    new ReportResource { ResourceType = context.ResourceTypes.First(), Amount = 1000 },
                    new ReportResource { ResourceType = context.ResourceTypes.Skip(1).First(), Amount = 2000 }
                },
                Round = 0
            });

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Fills the database with some test users, armies, buildings.
        /// </summary>
        /// <param name="context">The dbcontext to use.</param>
        /// <param name="count">The amount of users that should be generated.</param>
        /// <returns>The task representing the operation.</returns>
        public static async Task GenerateRandomTestUsersAsync(this UnderSeaDatabaseContext context, int count)
        {
            var globals = await context.GlobalValues.SingleAsync();
            globals.Round += globals.RandomEventGraceTimer + 1;
            var rng = new Random();

            var users = Enumerable.Range(1, count).Select(x => new User { UserName = Guid.NewGuid().ToString() }).ToList();
            var countries = users.Select(x => new Country
            {
                Resources = context.ResourceTypes.Select(r => new CountryResource { Amount = rng.Next(0, 50000), ResourceType = r }).ToList(),
                ParentUser = x,
                Name = x.UserName,
                InProgressResearches = context.ResearchTypes.Where(r => rng.NextDouble() < 0.5)
                    .Select(r => new InProgressResearch { TimeLeft = 1, Research = r }).ToList(),
                InProgressBuildings = context.BuildingTypes.Where(b => rng.NextDouble() < 0.5)
                    .Select(b => new InProgressBuilding { TimeLeft = 1, Building = b }).ToList(),
                Buildings = context.BuildingTypes.Where(b => rng.NextDouble() < 0.5)
                    .Select(b => new CountryBuilding { Count = rng.Next(1, 5), Building = b }).ToList()
            }).ToList();

            foreach (var country in countries)
            {
                var attackCount = rng.Next(0, 5);
                var attacks = new List<Command>
                {
                    new Command
                    {
                        TargetCountry = country,
                        Divisions = context.UnitTypes.Select(u => new Division { Count = rng.Next(1, 100), Unit = u }).ToList()
                    }
                };

                for (int iii = 0; iii < attackCount; iii++)
                {
                    var target = countries[rng.Next(countries.Count)];

                    if (attacks.Any(x => x.TargetCountry.Name.Equals(target.Name)))
                    {
                        iii--;
                    }
                    else
                    {
                        attacks.Add(new Command
                        {
                            TargetCountry = target,
                            Divisions = context.UnitTypes.Select(u => new Division { Count = rng.Next(1, 100), Unit = u }).ToList()
                        });
                    }
                }

                country.Commands = attacks;
            }

            context.Users.AddRange(users);
            context.Countries.AddRange(countries);
            await context.SaveChangesAsync();
        }
    }
}