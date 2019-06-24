using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Dal;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class CountryServiceTests
    {
        private UnderSeaDatabaseContext context;
        private ICountryService countryService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            countryService = new CountryService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestGetCountry(string username)
        {
            var info = await countryService.GetCountryInfoAsync(username);
            Assert.IsTrue(info.Pearls > 1000);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
