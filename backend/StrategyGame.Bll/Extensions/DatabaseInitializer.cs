using Microsoft.EntityFrameworkCore;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Creations;
using StrategyGame.Model.Entities.Effects;
using StrategyGame.Model.Entities.Frontend;
using StrategyGame.Model.Entities.Reports;
using StrategyGame.Model.Entities.Resources;
using StrategyGame.Model.Entities.Units;
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
            context.CountryResources.RemoveRange(context.CountryResources);
            context.BuildingResources.RemoveRange(context.BuildingResources);
            context.ResearchResources.RemoveRange(context.ResearchResources);
            context.UnitResources.RemoveRange(context.UnitResources);

            // Purge commands, divisions, units, reports
            context.Reports.RemoveRange(context.Reports);
            context.Commands.RemoveRange(context.Commands);
            context.Divisions.RemoveRange(context.Divisions);
            context.UnitTypes.RemoveRange(context.UnitTypes);

            // Purge contents
            context.BuildingContents.RemoveRange(context.BuildingContents);
            context.ResearchContents.RemoveRange(context.ResearchContents);
            context.ResearchContents.RemoveRange(context.ResearchContents);
            context.UnitContents.RemoveRange(context.UnitContents);
            context.EventContents.RemoveRange(context.EventContents);
            context.ResourceContents.RemoveRange(context.ResourceContents);

            // Purge "static" values
            context.BuildingTypes.RemoveRange(context.BuildingTypes);
            context.Countries.RemoveRange(context.Countries);
            context.Effects.RemoveRange(context.Effects);
            context.GlobalValues.RemoveRange(context.GlobalValues);
            context.ResearchTypes.RemoveRange(context.ResearchTypes);
            context.RandomEvents.RemoveRange(context.RandomEvents);
            context.ResourceTypes.RemoveRange(context.ResourceTypes);

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
            var spyCont = new UnitContent
            {
                Name = "Kém",
                Description = "Ha kémek segítségével információt gyűjthetsz az ellenségeidről."
            };

            context.UnitContents.AddRange(sealCont, ponyCont, lazorCont, leaderCont, spyCont);

            var mudTCont = new ResearchContent
            {
                Name = "Iszap traktor",
                Description = "Iszapozza a korallt, +10% korall termelés",
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
            var setCont = new ResearchContent
            {
                Name = "Telepesek",
                Description = "Telepeseket küld egy messzi régióba, akik új országot alapítanak"
            };

            context.ResearchContents.AddRange(mudTCont, mudCCont, defCont, attCont, cCont, taxCont, setCont);

            var plagueCont = new EventContent
            {
                Name = "Buborék-pestis",
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
            var coral = new ResourceType { Content = coralCont, StartingAmount = 500 };
            var pearl = new ResourceType { Content = pearlCont, StartingAmount = 1000 };
            var stone = new ResourceType { Content = stoneCont, StartingAmount = 1000 };
            context.ResourceTypes.AddRange(coral, pearl, stone);
            await context.SaveChangesAsync();

            // Effects, Buildings, researches
            // áramlásirányító
            var popIn = new Effect
            {
                Name = KnownValues.PopulationChange,
                Value = 50,
                Parameter = pearl.Id.ToString() + ":" + 25,
                IsOneTime = false
            };
            var cp = new Effect
            {
                Name = KnownValues.ResourceProductionChange,
                Value = 200,
                Parameter = coral.Id.ToString(),
                IsOneTime = false
            };
            var currentController = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, Child = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = currentCont
            };

            // zátonyvár
            var bsIn = new Effect
            {
                Name = KnownValues.BarrackSpaceChange,
                Value = 200,
                IsOneTime = false
            };
            var reefCastle = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, Child = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = reefCastCont
            };

            // kőbánya
            var stIn = new Effect
            {
                Name = KnownValues.ResourceProductionChange,
                Value = 200,
                Parameter = stone.Id.ToString(),
                IsOneTime = false
            };
            var stoneMine = new BuildingType
            {
                Cost = new[] { new BuildingResource { Amount = 1000, Child = stone } },
                BuildTime = 5,
                MaxCount = -1,
                Content = reefCastCont
            };

            // Iszaptraktor
            var harvMod1 = new Effect { Name = KnownValues.ResourceProductionModifier, Value = 0.1, Parameter = pearl.Id.ToString(), IsOneTime = false };
            var mudT = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = mudTCont };

            // Iszapkombájn
            var harvMod2 = new Effect { Name = KnownValues.ResourceProductionModifier, Value = 0.15, Parameter = pearl.Id.ToString(), IsOneTime = false };
            var mudC = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = mudCCont };

            // korallfal
            var defMod1 = new Effect { Name = KnownValues.UnitDefenseModifier, Value = 0.2, IsOneTime = false };
            var wall = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = defCont };

            // Szonárágyú
            var attMod1 = new Effect { Name = KnownValues.UnitAttackModifier, Value = 0.2, IsOneTime = false };
            var canon = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = attCont };

            // Harcművészet
            var combModA = new Effect { Name = KnownValues.UnitAttackModifier, Value = 0.1, IsOneTime = false };
            var combModD = new Effect { Name = KnownValues.UnitDefenseModifier, Value = 0.1, IsOneTime = false };
            var martialArts = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = cCont };

            // Alchemy
            var taxMod1 = new Effect { Name = KnownValues.ResourceProductionModifier, Value = 0.3, Parameter = pearl.Id.ToString(), IsOneTime = false };
            var alchemy = new ResearchType { Cost = new[] { new ResearchResource { Amount = 1000, Child = pearl } }, ResearchTime = 15, MaxCompletedAmount = 1, Content = taxCont };

            // Settlers
            var settlerEff = new Effect { Name = KnownValues.NewCountryEffect, IsOneTime = true };
            var settler = new ResearchType { Cost = new[] { new ResearchResource { Amount = 100000, Child = pearl } }, ResearchTime = 30, MaxCompletedAmount = 1, Content = setCont };

            // Add effects, buildings, researches
            context.Effects.AddRange(popIn, cp, bsIn, harvMod1, harvMod2,
                defMod1, attMod1, combModA, combModD, taxMod1, stIn);
            context.BuildingTypes.AddRange(currentController, reefCastle, stoneMine);
            context.ResearchTypes.AddRange(mudT, mudC, wall, canon, martialArts, alchemy, settler);
            await context.SaveChangesAsync();


            // Add effects to buildings and researches
            context.BuildingEffects.AddRange(
                new BuildingEffect { Parent = currentController, Child = popIn },
                new BuildingEffect { Parent = currentController, Child = cp },
                new BuildingEffect { Parent = reefCastle, Child = bsIn });

            context.ResearchEffects.AddRange(
                new ResearchEffect { Parent = mudT, Child = harvMod1 },
                new ResearchEffect { Parent = mudC, Child = harvMod2 },
                new ResearchEffect { Parent = wall, Child = defMod1 },
                new ResearchEffect { Parent = canon, Child = attMod1 },
                new ResearchEffect { Parent = martialArts, Child = combModA },
                new ResearchEffect { Parent = martialArts, Child = combModD },
                new ResearchEffect { Parent = alchemy, Child = taxMod1 },
                new ResearchEffect { Parent = settler, Child = settlerEff });
            await context.SaveChangesAsync();

            // Add units
            // rohamfóka
            var seal3 = new UnitType
            {
                AttackPower = 10,
                DefensePower = 5,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                IsPurchasable = false,
                CarryCapacity = 10,
                Content = sealCont
            };
            var seal2 = new UnitType
            {
                AttackPower = 8,
                DefensePower = 3,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                BattlesToRankUp = 5,
                RankedUpType = seal3,
                IsPurchasable = false,
                CarryCapacity = 8,
                Content = sealCont
            };
            var seal = new UnitType
            {
                AttackPower = 6,
                DefensePower = 2,
                Cost = new[]
                {
                    new UnitResource { Amount = 50, MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                BattlesToRankUp = 3,
                RankedUpType = seal2,
                IsPurchasable = true,
                CarryCapacity = 5,
                Content = sealCont
            };
            // csatacsikó
            var pony3 = new UnitType
            {
                AttackPower = 5,
                DefensePower = 10,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                IsPurchasable = false,
                CarryCapacity = 30,
                Content = ponyCont
            };
            var pony2 = new UnitType
            {
                AttackPower = 3,
                DefensePower = 8,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                BattlesToRankUp = 5,
                RankedUpType = pony3,
                IsPurchasable = false,
                CarryCapacity = 25,
                Content = ponyCont
            };
            var pony = new UnitType
            {
                AttackPower = 2,
                DefensePower = 6,
                Cost = new[]
                {
                    new UnitResource {Amount = 50, MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { Amount = 1, Child = coral }
                },
                BattlesToRankUp = 3,
                RankedUpType = pony2,
                IsPurchasable = true,
                CarryCapacity = 20,
                Content = ponyCont
            };
            // lézercápa
            var lazor3 = new UnitType
            {
                AttackPower = 10,
                DefensePower = 10,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 3, Child = pearl },
                    new UnitResource { MaintenanceAmount = 2, Child = coral }
                },
                IsPurchasable = false,
                CarryCapacity = 40,
                Content = lazorCont
            };
            var lazor2 = new UnitType
            {
                AttackPower = 7,
                DefensePower = 7,
                Cost = new[]
                {
                    new UnitResource { MaintenanceAmount = 1, Child = pearl },
                    new UnitResource { MaintenanceAmount = 1, Child = coral }
                },
                BattlesToRankUp = 5,
                RankedUpType = lazor3,
                IsPurchasable = false,
                CarryCapacity = 30,
                Content = lazorCont
            };
            var lazor = new UnitType
            {
                AttackPower = 5,
                DefensePower = 5,
                Cost = new[]
                {
                    new UnitResource {Amount = 100, MaintenanceAmount = 3, Child = pearl },
                    new UnitResource { MaintenanceAmount = 2, Child = coral }
                },
                BattlesToRankUp = 3,
                RankedUpType = lazor2,
                IsPurchasable = true,
                CarryCapacity = 20,
                Content = lazorCont
            };
            // hadvezér
            var leader = new LeaderType
            {
                AttackPower = 0,
                DefensePower = 0,
                Cost = new[]
                {
                    new UnitResource { Amount = 400, MaintenanceAmount = 4, Child = pearl },
                    new UnitResource { MaintenanceAmount = 2, Child = coral }
                },
                BattlesToRankUp = 3,
                IsPurchasable = true,
                Content = leaderCont
            };
            // kém
            var spy = new SpyType
            {
                AttackPower = 0,
                DefensePower = 0,
                Cost = new []
                {
                    new UnitResource {Amount = 50, MaintenanceAmount = 1, Child = pearl}, 
                    new UnitResource {Amount = 0, MaintenanceAmount = 1, Child = coral}, 
                },
                IsPurchasable = true,
                Content = spyCont
            };

            context.UnitTypes.AddRange(seal3, seal2, seal, pony3, pony2, pony, lazor3, lazor2, lazor, leader, spy);
            await context.SaveChangesAsync();


            // Add events
            var plague = new RandomEvent { Content = plagueCont };
            var removeCurrent = new Effect
            { Parameter = currentController.Id.ToString(), Name = KnownValues.AddRemoveBuildingEffect, Value = -1, IsOneTime = true };

            var fire = new RandomEvent { Content = fireCont };
            var removeCastle = new Effect
            { Parameter = reefCastle.Id.ToString(), Name = KnownValues.AddRemoveBuildingEffect, Value = -1, IsOneTime = true };

            var mine = new RandomEvent { Content = mineCont };
            var addPearl = new Effect
            { Value = 1000, Name = KnownValues.ResourceProductionChange, Parameter = pearl.Id.ToString() };

            var goodHarvest = new RandomEvent { Content = goodhvCont };
            var extraCoral = new Effect
            {
                Name = KnownValues.BuildingProductionChange,
                Value = 50,
                Parameter = currentController.Id.ToString() + ";" + coral.Id.ToString()
            };

            var badHarvest = new RandomEvent { Content = badhvCont };
            var lessCoral = new Effect
            {
                Name = KnownValues.BuildingProductionChange,
                Value = -50,
                Parameter = currentController.Id.ToString() + ";" + coral.Id.ToString()
            };

            var contentPopulation = new RandomEvent { Content = contPopCont };
            var addCurrent = new Effect
            { Name = KnownValues.AddRemoveBuildingEffect, Parameter = currentController.Id.ToString(), Value = 1, IsOneTime = true };
            var discontentPopulation = new RandomEvent();

            var contentSoldiers = new RandomEvent { Content = contSolCont };
            var addAttack = new Effect
            { Name = KnownValues.UnitAttackChange, Value = 1 };

            var discontentSoldiers = new RandomEvent { Content = disconSolCont };
            var removeAttack = new Effect
            { Name = KnownValues.UnitAttackChange, Value = -1 };

            await context.SaveChangesAsync();

            // Add event effects
            context.EventEffects.AddRange(
                new EventEffect { Child = removeCurrent, Parent = plague },
                new EventEffect { Child = removeCastle, Parent = fire },
                new EventEffect { Child = addPearl, Parent = mine },
                new EventEffect { Child = extraCoral, Parent = goodHarvest },
                new EventEffect { Child = lessCoral, Parent = badHarvest },
                new EventEffect { Child = addCurrent, Parent = contentPopulation },
                new EventEffect { Child = removeCurrent, Parent = discontentPopulation },
                new EventEffect { Child = addAttack, Parent = contentSoldiers },
                new EventEffect { Child = removeAttack, Parent = discontentSoldiers }
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
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = 0, Child = x }).ToList()
            };
            var rc = new Country
            {
                Name = "rich",
                ParentUser = theRich,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = 100000, Child = x }).ToList()
            };
            var cc = new Country
            {
                Name = "attacky",
                ParentUser = theCommander,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, Child = x }).ToList()
            };
            var bc = new Country
            {
                Name = "poi",
                ParentUser = theBuilder,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, Child = x }).ToList()
            };
            var sc = new Country
            {
                Name = "science",
                ParentUser = theResearcher,
                Resources = context.ResourceTypes.Select(x => new CountryResource { Amount = x.StartingAmount, Child = x }).ToList()
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
                Child = b1,
                Amount = 1,
                Parent = bc
            }, new CountryBuilding
            {
                Child = b2,
                Amount = 13,
                Parent = bc
            });

            context.InProgressBuildings.Add(new InProgressBuilding
            { Child = context.BuildingTypes.First(), Parent = cc, TimeLeft = 1 });
            context.InProgressResearches.Add(new InProgressResearch
            { Child = context.ResearchTypes.First(), Parent = cc, TimeLeft = 1 });

            var r1 = await context.ResearchTypes.FirstAsync();
            var r2 = await context.ResearchTypes.Skip(3).FirstAsync();

            context.CountryResearches.AddRange(new CountryResearch
            {
                Child = r1,
                Amount = 1,
                Parent = sc
            }, new CountryResearch
            {
                Child = r2,
                Amount = 1,
                Parent = sc
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
                    new ReportResource { Child = context.ResourceTypes.First(), Amount = 1000 },
                    new ReportResource { Child = context.ResourceTypes.Skip(1).First(), Amount = 2000 }
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
                Resources = context.ResourceTypes.Select(r => new CountryResource { Amount = rng.Next(0, 50000), Child = r }).ToList(),
                ParentUser = x,
                Name = x.UserName,
                InProgressResearches = context.ResearchTypes.Where(r => rng.NextDouble() < 0.5)
                    .Select(r => new InProgressResearch { TimeLeft = 1, Child = r }).ToList(),
                InProgressBuildings = context.BuildingTypes.Where(b => rng.NextDouble() < 0.5)
                    .Select(b => new InProgressBuilding { TimeLeft = 1, Child = b }).ToList(),
                Buildings = context.BuildingTypes.Where(b => rng.NextDouble() < 0.5)
                    .Select(b => new CountryBuilding { Amount = rng.Next(1, 5), Child = b }).ToList()
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