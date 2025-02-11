﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Units;
using StrategyGame.Dal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class UnitServiceTests
    {
        private UnderSeaDatabaseContext context;
        private IUnitService unitService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            unitService = new UnitService(context, new AsyncReaderWriterLock(), ModifierParserContainer.CreateDefault(),
                UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheBuilder")]
        public async Task TestBuyUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            var units = await unitService.CreateUnitAsync(username, new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
            Assert.AreEqual(units.Single(u => u.Id == id).TotalCount, 10);
        }

        [TestMethod]
        [DataRow("TheBuilder")]
        public async Task TestDeleteUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
            await unitService.DeleteUnitsAsync(username, id, 10);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestBuyUnitNoMoney(string username)
        {
            var globals = await context.GlobalValues.SingleAsync();
            globals.StartingBarrackSpace = 1000;
            await context.SaveChangesAsync();

            var id = (await context.UnitTypes.FirstAsync()).Id;
            await unitService.CreateUnitAsync(username, new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(LimitReachedException))]
        public async Task TestBuyUnitNoSpace(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, new[] { new PurchaseDetails { UnitId = id, Count = 200 } });
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}