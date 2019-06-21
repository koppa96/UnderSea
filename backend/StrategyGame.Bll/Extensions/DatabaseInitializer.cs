using Microsoft.EntityFrameworkCore;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Frontend;
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

            // Purge contents
            context.BuildingContents.RemoveRange(context.BuildingContents);
            context.ResearchContents.RemoveRange(context.ResearchContents);
            context.UnitContents.RemoveRange(context.UnitContents);

            // Purge commands, divisions, and units
            context.Commands.RemoveRange(context.Commands);
            context.Divisions.RemoveRange(context.Divisions);
            context.UnitTypes.RemoveRange(context.UnitTypes);

            // Purge "static" values
            context.BuildingTypes.RemoveRange(context.BuildingTypes);
            context.Countries.RemoveRange(context.Countries);
            context.Effects.RemoveRange(context.Effects);
            context.GlobalValues.RemoveRange(context.GlobalValues);
            context.ResearchTypes.RemoveRange(context.ResearchTypes);

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
            // Effects, Buildings, researches
            // áramlásirányító
            var popIn = new Effect() { Name = KnownValues.PopulationIncrease, Value = 50 };
            var cp = new Effect() { Name = KnownValues.CoralProductionIncrease, Value = 200 };
            var currentController = new BuildingType() { CostPearl = 1000, CostCoral = 0, BuildTime = 5, MaxCount = -1 };

            // zátonyvár
            var bsIn = new Effect() { Name = KnownValues.BarrackSpaceIncrease, Value = 200 };
            var reefCastle = new BuildingType() { CostPearl = 1000, CostCoral = 0, BuildTime = 5, MaxCount = -1 };

            // Iszaptraktor
            var harvMod1 = new Effect() { Name = KnownValues.HarvestModifier, Value = 0.1 };
            var mudT = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };

            // Iszapkombájn
            var harvMod2 = new Effect() { Name = KnownValues.HarvestModifier, Value = 0.15 };
            var mudC = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };

            // korallfal
            var defMod1 = new Effect() { Name = KnownValues.UnitDefenseModifier, Value = 0.2 };
            var wall = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };

            // Szonárágyú
            var attMod1 = new Effect() { Name = KnownValues.UnitAttackModifier, Value = 0.2 };
            var canon = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };

            // Harcművészet
            var combModA = new Effect() { Name = KnownValues.UnitAttackModifier, Value = 0.1 };
            var combModD = new Effect() { Name = KnownValues.UnitDefenseModifier, Value = 0.1 };
            var martialArts = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };

            // Alchemy
            var taxMod1 = new Effect() { Name = KnownValues.TaxationModifier, Value = 0.3 };
            var alchemy = new ResearchType() { CostPearl = 0, CostCoral = 0, ResearchTime = 15, MaxCompletedAmount = 1 };


            // Add effects, buildings, researches
            context.Effects.AddRange(popIn, cp, bsIn, harvMod1, harvMod2,
                defMod1, attMod1, combModA, combModD, taxMod1);
            context.BuildingTypes.AddRange(currentController, reefCastle);
            context.ResearchTypes.AddRange(mudT, mudC, wall, canon, martialArts, alchemy);
            await context.SaveChangesAsync();


            // Add effects to buildings and researches
            context.BuildingEffects.AddRange(
                new BuildingEffect() { Building = currentController, Effect = popIn },
                new BuildingEffect() { Building = currentController, Effect = cp },
                new BuildingEffect() { Building = reefCastle, Effect = bsIn });

            context.ResearchEffects.AddRange(
                new ResearchEffect() { Research = mudT, Effect = harvMod1 },
                new ResearchEffect() { Research = mudC, Effect = harvMod2 },
                new ResearchEffect() { Research = wall, Effect = defMod1 },
                new ResearchEffect() { Research = canon, Effect = attMod1 },
                new ResearchEffect() { Research = martialArts, Effect = combModA },
                new ResearchEffect() { Research = martialArts, Effect = combModD },
                new ResearchEffect() { Research = alchemy, Effect = taxMod1 });
            await context.SaveChangesAsync();


            // Add units
            // rohamfóka
            var seal = new UnitType()
            { AttackPower = 6, DefensePower = 2, CostPearl = 50, CostCoral = 0, MaintenancePearl = 1, MaintenanceCoral = 1 };
            // csatacsikó
            var pony = new UnitType()
            { AttackPower = 2, DefensePower = 6, CostPearl = 50, CostCoral = 0, MaintenancePearl = 1, MaintenanceCoral = 1 };
            // lézercápa
            var lazor = new UnitType()
            { AttackPower = 5, DefensePower = 5, CostPearl = 100, CostCoral = 0, MaintenancePearl = 3, MaintenanceCoral = 2 };

            context.UnitTypes.AddRange(seal, pony, lazor);
            await context.SaveChangesAsync();


            // Add contents
            var currentCont = new BuildingContent()
            {
                Parent = currentController,
                Name = "Áramlásirányító",
                Description = "+50 lakos, 200 korall / kör",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTpStWdiTJFGARYo6i93yeO0pHA0EQYJelOifiWIPmP7qveLS6n"
            };

            var reefCastCont = new BuildingContent()
            {
                Parent = reefCastle,
                Name = "Zátonyvár",
                Description = "+200 szállás",
                ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/02/72/f4/54/filename-pict0458-jpg.jpg"
            };

            context.BuildingContents.AddRange(currentCont, reefCastCont);

            var sealCont = new UnitContent()
            {
                Parent = seal,
                Name = "Rohamfóka",
                Description = "Jól támad de rosszul véd",
                ImageUrl = "https://resources.stuff.co.nz/content/dam/images/1/t/a/s/4/o/image.related.StuffLandscapeSixteenByNine.710x400.1tankf.png/1546211918775.jpg"
            };
            var ponyCont = new UnitContent()
            {
                Parent = pony,
                Name = "Csatacsikó",
                Description = "Jól véd de rosszul támad",
                ImageUrl = "http://www2.padi.com/blog/wp-content/uploads/2013/08/seahorse.jpg"
            };
            var lazorCont = new UnitContent()
            {
                Parent = lazor,
                Name = "Lézercápa",
                Description = "lazers man",
                ImageUrl = "https://vignette.wikia.nocookie.net/venturian-battle-headquarters/images/6/69/Flyinglasershark.jpg/revision/latest?cb=20160714220743"
            };

            context.UnitContents.AddRange(sealCont, ponyCont, lazorCont);

            var mudTCont = new ResearchContent()
            {
                Parent = mudT,
                Name = "Iszap traktor",
                Description = "Iszapozza a korallt (amitől amúgy IRL meghalna, korall nem növény nem kell neki föld), +10% korall termelés",
                ImageUrl = "https://cdn.pixabay.com/photo/2017/10/09/09/55/mud-2832910_960_720.jpg"
            };
            var mudCCont = new ResearchContent()
            {
                Parent = mudC,
                Name = "Iszap kombájn",
                Description = "Nagyon iszapozza a korallt, +15% korall termelés",
                ImageUrl = "https://secure.i.telegraph.co.uk/multimedia/archive/03350/glastonbury-mud-sp_3350460k.jpg"
            };
            var defCont = new ResearchContent()
            {
                Parent = wall,
                Name = "Korallfal",
                Description = "Fal, korallból. +20% védekezés",
                ImageUrl = "https://ak2.picdn.net/shutterstock/videos/1396612/thumb/1.jpg"
            };
            var attCont = new ResearchContent()
            {
                Parent = canon,
                Name = "Szonárágyú",
                Description = "Mint a denevér, echo-lokáció. +20% támadás",
                ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/07/24/69/da/dive-abaco.jpg"
            };
            var cCont = new ResearchContent()
            {
                Parent = martialArts,
                Name = "Vízalatti harcművészetek",
                Description = "\"A különbség a lehetetlen és a lehetséges között az egyén akarata.\", +10% védekezés és támadás",
                ImageUrl = "https://www.pallensmartialarts.com/uploads/1/0/9/3/109303993/girl-kicking-boy-to-air_1_orig.jpg"
            };
            var taxCont = new ResearchContent()
            {
                Parent = alchemy,
                Name = "Alkímia",
                Description = "A népesség pénzt csinál, +30% adó bevétel",
                ImageUrl = "https://f4.bcbits.com/img/a3431451072_10.jpg"
            };

            context.ResearchContents.AddRange(mudTCont, mudCCont, defCont, attCont, cCont, taxCont);
            await context.SaveChangesAsync();


            // globals
            context.GlobalValues.Add(new GlobalValue()
            {
                BaseTaxation = 25,
                Round = 1,
                StartingBarrackSpace = 100,
                StartingCorals = 500,
                StartingPearls = 500,
                StartingPopulation = 10
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
            var thePoor = new User()
            { UserName = "ThePoor", Email = "poor@test.com" };
            var theRich = new User()
            { UserName = "TheRich", Email = "rich@test.com" };
            var theCommander = new User()
            { UserName = "TheCommander", Email = "comm@test.com" };
            var theBuilder = new User()
            { UserName = "TheBuilder", Email = "build@test.com" };
            context.AddRange(thePoor, theRich, theCommander, theBuilder);

            var pc = new Country()
            { Name = "poor", Corals = 10, Pearls = 10, ParentUser = thePoor };
            var rc = new Country()
            { Name = "rich", Corals = 1000000, Pearls = 1000000, ParentUser = theRich };
            var cc = new Country()
            { Name = "attacky", Corals = 1000, Pearls = 1000, ParentUser = theCommander };
            var bc = new Country()
            { Name = "poi", Corals = 1000, Pearls = 1000, ParentUser = theBuilder };
            context.Countries.AddRange(pc, rc, cc, bc);

            var d1 = new Command() { ParentCountry = pc, TargetCountry = pc };
            var d2 = new Command() { ParentCountry = rc, TargetCountry = rc };
            var d3 = new Command() { ParentCountry = cc, TargetCountry = cc };
            var d4 = new Command() { ParentCountry = bc, TargetCountry = bc };
            context.Commands.AddRange(d1, d2, d3, d4);

            var b1 = await context.BuildingTypes.FirstAsync().ConfigureAwait(false);
            var b2 = await context.BuildingTypes.Skip(1).FirstAsync().ConfigureAwait(false);

            context.CountryBuildings.AddRange(new CountryBuilding()
            {
                Building = b1,
                Count = 1,
                ParentCountry = bc
            }, new CountryBuilding()
            {
                Building = b2,
                Count = 13,
                ParentCountry = bc
            });

            var u1 = await context.UnitTypes.FirstAsync().ConfigureAwait(false);
            var u2 = await context.UnitTypes.Skip(1).FirstAsync().ConfigureAwait(false);
            var u3 = await context.UnitTypes.Skip(2).FirstAsync().ConfigureAwait(false);

            context.Divisions.AddRange(new Division
            {
                Count = 50,
                ParentCommand = d3,
                Unit = u1
            }, new Division()
            {
                Count = 10,
                ParentCommand = d3,
                Unit = u2
            }, new Division()
            {
                Count = 1,
                ParentCommand = d3,
                Unit = u3
            });

            await context.SaveChangesAsync();
        }
    }
}