﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            unitService = new UnitService(context,
                new ModifierParserContainer(new AbstractEffectModifierParser[]
                {
                    new BarrackSpaceEffectParser(),
                    new CoralProductionEffectParser(),
                    new PearlProductionEffectParser(),
                    new HarvestModifierEffectParser(),
                    new PopulationEffectParser(),
                    new ResourceModifierEffectParser(),
                    new UnitDefenseEffectParser(),
                    new UnitAttackEffectParser(),
                    new AddBuildingEffectParser(),
                    new IncreaseUnitAttackEffectParser(),
                    new BuildingCoralProductionEffectParser()
                }), UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheCommander")]
        public async Task TestGetUnitInfo(string username)
        {
            var units = await unitService.GetUnitInfoAsync(username, 
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id);
            Assert.IsTrue(units.All(u => u.Count > 0));
        }

        [TestMethod]
        [DataRow("TheBuilder")]
        public async Task TestBuyUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
            var units = await unitService.GetUnitInfoAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id);
            Assert.AreEqual(units.Single(u => u.Id == id).Count, 10);
        }

        [TestMethod]
        [DataRow("TheBuilder")]
        public async Task TestDeleteUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, 
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
            await unitService.DeleteUnitsAsync(username, 
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id, id, 10);
            var units = await unitService.GetUnitInfoAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id);
            Assert.AreEqual(units.Single(u => u.Id == id).Count, 0);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestBuyUnitNoMoney(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                new[] { new PurchaseDetails { UnitId = id, Count = 10 } });
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(LimitReachedException))]
        public async Task TestBuyUnitNoSpace(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                new[] { new PurchaseDetails { UnitId = id, Count = 200 } });
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}