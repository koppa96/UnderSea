using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;
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
        public void Initialize()
        {
            context = UtilityFactory.CreateContext();
            context.Database.EnsureCreated();
            context.FillWithDefaultAsync().Wait();
            context.AddTestUsersAsync().Wait();

            buildingService = new BuildingService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        public async Task TestGetUserBuildings()
        {
            var user = await context.Users.SingleAsync(u => u.UserName == "TheBuilder");

            var buildings = await buildingService.GetBuildingsAsync(user.UserName);

            var dbBuildings = await context.CountryBuildings.Include(cb => cb.Building)
                .Where(cb => cb.ParentCountry.ParentUser.UserName == user.UserName)
                .ToListAsync();

            foreach (var buildingInfo in buildings)
            {
                var dbBuilding = dbBuildings.SingleOrDefault(cb => cb.Building.Id == buildingInfo.Id);
                
                if (dbBuilding == null)
                {
                    Assert.AreEqual(0, buildingInfo.Count);
                }
                else
                {
                    Assert.AreEqual(dbBuilding.Count, buildingInfo.Count);
                }
            }
        }

        [TestMethod]
        public async Task TestStartBuilding()
        {
            var user = await context.Users.SingleAsync(u => u.UserName == "TheRich");
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(user.UserName, buildingId);

            var inProgressBuildings = await context.InProgressBuildings
                .Include(b => b.Building)
                .Where(b => b.ParentCountry.ParentUser.UserName == user.UserName)
                .ToListAsync();

            Assert.AreEqual(1, inProgressBuildings.Count);
            Assert.AreEqual(buildingId, inProgressBuildings.First().Building.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestStartBuildingNoMoney()
        {
            var user = await context.Users.SingleAsync(u => u.UserName == "ThePoor");
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(user.UserName, buildingId);
        }

        [TestMethod]
        [ExpectedException(typeof(InProgressException))]
        public async Task TestStartBuildingAlreadyBuilding()
        {
            var user = await context.Users.SingleAsync(u => u.UserName == "TheRich");
            var buildingId = (await context.BuildingTypes.FirstAsync()).Id;

            await buildingService.StartBuildingAsync(user.UserName, buildingId);
            await buildingService.StartBuildingAsync(user.UserName, buildingId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestStartBuildingNotExisting()
        {
            var user = await context.Users.SingleAsync(u => u.UserName == "TheRich");

            await buildingService.StartBuildingAsync(user.UserName, -1);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}