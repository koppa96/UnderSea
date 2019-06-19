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
                Description = "+50 lakos, 200 korall / kör"
            };

            var reefCastCont = new BuildingContent(reefCastle)
            {
                Name = "Zátonyvár",
                Description = "+200 szállás"
            };

            context.BuildingContents.AddRange(currentCont, reefCastCont);

            var sealCont = new UnitContent(seal)
            {
                Name = "Rohamfóka",
                Description = "Jól támad de rosszul véd"
            };
            var ponyCont = new UnitContent(pony)
            {
                Name = "Csatacsikó",
                Description = "Jól véd de rosszul támad"
            };
            var lazorCont = new UnitContent(lazor)
            {
                Name = "Lézercápa",
                Description = "lazers man"
            };

            context.UnitContents.AddRange(sealCont, ponyCont, lazorCont);

            var mudTCont = new ResearchContent(mudT)
            {
                Name = "Iszap traktor",
                Description = "Iszapozza a korallt (amitől amúgy IRL meghalna, korall nem növény nem kell neki föld), +10% korall termelés"
            };
            var mudCCont = new ResearchContent(mudC)
            {
                Name = "Iszap kombájn",
                Description = "Nagyon iszapozza a korallt, +15% korall termelés"
            };
            var defCont = new ResearchContent(wall)
            {
                Name = "Korallfal",
                Description = "Fal, korallból. +20% védekezés"
            };
            var attCont = new ResearchContent(canon)
            {
                Name = "Szonárágyú",
                Description = "Mint a denevér, echo-lokáció. +20% támadás"
            };
            var cCont = new ResearchContent(martialArts)
            {
                Name = "Vízalatti harcművészetek",
                Description = "\"A különbség a lehetetlen és a lehetséges között az egyén akarata.\", +10% védekezés és támadás"
            };
            var taxCont = new ResearchContent(alchemy)
            {
                Name = "Alkímia",
                Description = "A népesség pénzt csinál, +30% adó bevétel"
            };

            context.ResearchContents.AddRange(mudTCont, mudCCont, defCont, attCont, cCont, taxCont);
            context.SaveChanges();
        }
    }
}