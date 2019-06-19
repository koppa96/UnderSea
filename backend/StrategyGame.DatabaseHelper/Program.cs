using Microsoft.EntityFrameworkCore;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Frontend;
using System;

namespace StrategyGame.DatabaseHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UnderSeaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            Console.WriteLine("Connecting to database with connection string: " + connString);

            var builder = new DbContextOptionsBuilder<UnderSeaDatabase>();
            // local DB
            builder.UseSqlServer(connString);

            using (var context = new UnderSeaDatabase(builder.Options))
            {
                Console.WriteLine("Connected to the database.");

                PurgeDatabase(context);
                FillWithDefault(context);

                //context.SaveChanges();
            }

            Console.Write("Finished, press any key to exit...");
            Console.ReadKey(true);
        }

        static void PurgeDatabase(UnderSeaDatabase context)
        {
            Console.WriteLine("Purging the database...");

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
            context.SaveChanges();

            Console.WriteLine("Database purged!");
        }

        static void FillWithDefault(UnderSeaDatabase context)
        {
            Console.WriteLine("Adding default data...");

            // Effects, Buildings, researches
            // áramlásirányító
            var popIn = new Effect("population-increase", 50);
            var cp = new Effect("coral-production", 200);
            var currentController = new BuildingType(1000, 0, 5, -1);

            // zátonyvár
            var bsIn = new Effect("barrack-space-increase", 200);
            var reefCastle = new BuildingType(1000, 0, 5, -1);

            // Iszaptraktor
            var harvMod1 = new Effect("harvest-modifier", 0.1);
            var mudT = new ResearchType(0, 0, 15, 1);

            // Iszapkombájn
            var harvMod2 = new Effect("harvest-modifier", 0.15);
            var mudC = new ResearchType(0, 0, 15, 1);

            // korallfal
            var defMod1 = new Effect("unit-defense", 0.2);
            var wall = new ResearchType(0, 0, 15, 1);

            // Szonárágyú
            var attMod1 = new Effect("unit-attack", 0.2);
            var canon = new ResearchType(0, 0, 15, 1);

            // Harcművészet
            var combModA = new Effect("unit-attack", 0.1);
            var combModD = new Effect("unit-defense", 0.1);
            var martialArts = new ResearchType(0, 0, 15, 1);

            // Alchemy
            var taxMod1 = new Effect("taxation-modifier", 0.3);
            var alchemy = new ResearchType(0, 0, 15, 1);


            // Add effects, buildings, researches
            context.Effects.AddRange(popIn, cp, bsIn, harvMod1, harvMod2,
                defMod1, attMod1, combModA, combModD, taxMod1);
            context.BuildingTypes.AddRange(currentController, reefCastle);
            context.ResearchTypes.AddRange(mudT, mudC, wall, canon, martialArts, alchemy);
            context.SaveChanges();


            // Add effects to buildings and researches
            context.BuildingEffects.AddRange(new BuildingEffect(currentController, popIn),
                new BuildingEffect(currentController, cp), new BuildingEffect(reefCastle, bsIn));

            context.ResearchEffects.AddRange(new ResearchEffect(mudT, harvMod1), new ResearchEffect(mudC, harvMod2),
                new ResearchEffect(wall, defMod1), new ResearchEffect(canon, attMod1), new ResearchEffect(martialArts, combModA),
                new ResearchEffect(martialArts, combModD), new ResearchEffect(alchemy, taxMod1));
            context.SaveChanges();


            // Add units
            // rohamfóka
            var seal = new UnitType(6, 2, 50, 0, 1, 1);
            // csatacsikó
            var pony = new UnitType(2, 6, 50, 0, 1, 1);
            // lézercápa
            var lazor = new UnitType(5, 5, 100, 0, 3, 2);

            context.UnitTypes.AddRange(seal, pony, lazor);
            context.SaveChanges();


            // Add contents
            var currentCont = new BuildingContent(currentController)
            {
                Name = "Áramlásirányító",
                Description = "+50 lakos, 200 korall / kör",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTpStWdiTJFGARYo6i93yeO0pHA0EQYJelOifiWIPmP7qveLS6n"
            };

            var reefCastCont = new BuildingContent(reefCastle)
            {
                Name = "Zátonyvár",
                Description = "+200 szállás",
                ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/02/72/f4/54/filename-pict0458-jpg.jpg"
            };

            context.BuildingContents.AddRange(currentCont, reefCastCont);

            var sealCont = new UnitContent(seal)
            {
                Name = "Rohamfóka",
                Description = "Jól támad de rosszul véd",
                ImageUrl = "https://resources.stuff.co.nz/content/dam/images/1/t/a/s/4/o/image.related.StuffLandscapeSixteenByNine.710x400.1tankf.png/1546211918775.jpg"
            };
            var ponyCont = new UnitContent(pony)
            {
                Name = "Csatacsikó",
                Description = "Jól véd de rosszul támad",
                ImageUrl = "http://www2.padi.com/blog/wp-content/uploads/2013/08/seahorse.jpg"
            };
            var lazorCont = new UnitContent(lazor)
            {
                Name = "Lézercápa",
                Description = "lazers man",
                ImageUrl = "https://vignette.wikia.nocookie.net/venturian-battle-headquarters/images/6/69/Flyinglasershark.jpg/revision/latest?cb=20160714220743"
            };

            context.UnitContents.AddRange(sealCont, ponyCont, lazorCont);

            var mudTCont = new ResearchContent(mudT)
            {
                Name = "Iszap traktor",
                Description = "Iszapozza a korallt (amitől amúgy IRL meghalna, korall nem növény nem kell neki föld), +10% korall termelés",
                ImageUrl = "https://cdn.pixabay.com/photo/2017/10/09/09/55/mud-2832910_960_720.jpg"
            };
            var mudCCont = new ResearchContent(mudC)
            {
                Name = "Iszap kombájn",
                Description = "Nagyon iszapozza a korallt, +15% korall termelés",
                ImageUrl = "https://secure.i.telegraph.co.uk/multimedia/archive/03350/glastonbury-mud-sp_3350460k.jpg"
            };
            var defCont = new ResearchContent(wall)
            {
                Name = "Korallfal",
                Description = "Fal, korallból. +20% védekezés",
                ImageUrl = "https://ak2.picdn.net/shutterstock/videos/1396612/thumb/1.jpg"
            };
            var attCont = new ResearchContent(canon)
            {
                Name = "Szonárágyú",
                Description = "Mint a denevér, echo-lokáció. +20% támadás",
                ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/07/24/69/da/dive-abaco.jpg"
            };
            var cCont = new ResearchContent(martialArts)
            {
                Name = "Vízalatti harcművészetek",
                Description = "\"A különbség a lehetetlen és a lehetséges között az egyén akarata.\", +10% védekezés és támadás",
                ImageUrl = "https://www.pallensmartialarts.com/uploads/1/0/9/3/109303993/girl-kicking-boy-to-air_1_orig.jpg"
            };
            var taxCont = new ResearchContent(alchemy)
            {
                Name = "Alkímia",
                Description = "A népesség pénzt csinál, +30% adó bevétel",
                ImageUrl = "https://f4.bcbits.com/img/a3431451072_10.jpg"
            };

            context.ResearchContents.AddRange(mudTCont, mudCCont, defCont, attCont, cCont, taxCont);
            context.SaveChanges();


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
            context.SaveChanges();
        }
    }
}