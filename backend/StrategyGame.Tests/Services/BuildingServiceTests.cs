using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Buildings;
using StrategyGame.Dal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class BuildingServiceTests
    {
        private IBuildingService buildingService;
        private UnderSeaDatabaseContext context;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();

            buildingService = new BuildingService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestStartBuilding(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username, buildingId);

            var inProgressBuildings = await context.InProgressBuildings
                .Include(b => b.Building)
                .Where(b => b.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            Assert.AreEqual(1, inProgressBuildings.Count);
            Assert.AreEqual(buildingId, inProgressBuildings.Single().Building.Id);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestStartBuildingNoMoney(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username, buildingId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(InProgressException))]
        public async Task TestStartBuildingAlreadyBuilding(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username, buildingId);
            await buildingService.StartBuildingAsync(username, buildingId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestStartBuildingNotExisting(string username)
        {
            await buildingService.StartBuildingAsync(username, -1);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}