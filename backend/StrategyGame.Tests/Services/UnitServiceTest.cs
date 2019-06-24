using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.Units;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class UnitServiceTest
    {
        private UnderSeaDatabaseContext context;
        private IUnitService unitService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            unitService = new UnitService(UtilityFactory.CreateMapper(), context,
                new ModifierParserContainer(new AbstractEffectModifierParser[]
                {
                    new BarrackSpaceEffectParser(),
                    new CoralProductionEffectParser(),
                    new HarvestModifierEffectParser(),
                    new PopulationEffectParser(),
                    new TaxModifierEffectParser(),
                    new UnitDefenseEffectParser(),
                    new UnitAttackEffectParser()
                }));
        }

        [TestMethod]
        [DataRow("TheCommander")]
        public async Task TestGetUnitInfo(string username)
        {
            var units = await unitService.GetUnitInfoAsync(username);
            Assert.IsTrue(units.All(u => u.Count > 0));
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestBuyUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, id, 10);
            var units = await unitService.GetUnitInfoAsync(username);
            Assert.AreEqual(units.Single(u => u.Id == id).Count, 10);
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestDeleteUnit(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, id, 10);
            await unitService.DeleteUnitsAsync(username, id, 10);
            var units = await unitService.GetUnitInfoAsync(username);
            Assert.AreEqual(units.Single(u => u.Id == id).Count, 0);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestBuyUnitNoMoney(string username)
        {
            var id = (await context.UnitTypes.FirstAsync()).Id;

            await unitService.CreateUnitAsync(username, id, 10);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}