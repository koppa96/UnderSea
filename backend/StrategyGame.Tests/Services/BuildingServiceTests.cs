using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;
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

            buildingService = new BuildingService(context, new AsyncReaderWriterLock(), UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestStartBuilding(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                buildingId);

            var inProgressBuildings = await context.InProgressBuildings
                .Include(b => b.Building)
                .Where(b => b.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            Assert.AreEqual(1, inProgressBuildings.Count);
            Assert.AreEqual(buildingId, inProgressBuildings.Single().Building.Id);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestStartBuildingNoMoney(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                buildingId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(InProgressException))]
        public async Task TestStartBuildingAlreadyBuilding(string username)
        {
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                buildingId);
            await buildingService.StartBuildingAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                buildingId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestStartBuildingDifferentType(string username)
        {
            var countryId = (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id;

            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;
            var otherBuildingId = (await context.BuildingTypes.FirstAsync(b => b.Id != buildingId)).Id;

            await buildingService.StartBuildingAsync(username, countryId, buildingId);
            await buildingService.StartBuildingAsync(username, countryId, otherBuildingId);

            var inProgress = await context.InProgressBuildings
                .Where(b => b.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            Assert.AreEqual(2, inProgress.Count);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestStartBuildingNotExisting(string username)
        {
            await buildingService.StartBuildingAsync(username,
                (await context.Countries.FirstAsync(c => c.ParentUser.UserName == username)).Id,
                -1);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}