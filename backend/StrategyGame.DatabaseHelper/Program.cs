﻿using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using System;

namespace StrategyGame.DatabaseHelper
{
    class Program
    {
        static void Main()
        {
            var connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UnderSeaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            Console.WriteLine("Connecting to database with connection string: " + connString);

            var builder = new DbContextOptionsBuilder<UnderSeaDatabaseContext>();
            // local DB
            builder.UseSqlServer(connString);

            using (var context = new UnderSeaDatabaseContext(builder.Options))
            {
                Console.WriteLine("Connected to the database.");
                context.PurgeDatabaseAsync().Wait();
                context.FillWithDefaultAsync().Wait();
            }

            Console.Write("Finished, press any key to exit...");
            Console.ReadKey(true);
        }
    }
}