using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Services.Buildings;
using StrategyGame.Dal;
using System;
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
            buildingService = new BuildingService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        public async Task TestGetUserBuildings()
        {

        }

        [TestMethod]
        public async Task TestStartBuilding()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestStartBuildingNoMoney()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(InProgressException))]
        public async Task TestStartBuildingAlreadyBuilding()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestStartBuildingNotExisting()
        {

        }
    }
}