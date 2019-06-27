using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.Country;
using StrategyGame.Dal;
using System.Linq;
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
            countryService = new CountryService(UtilityFactory.CreateMapper(), context,
                new ModifierParserContainer(new AbstractEffectModifierParser[]
                {
                    new BarrackSpaceEffectParser(),
                    new CoralProductionEffectParser(),
                    new PearlProductionEffectParser(),
                    new HarvestModifierEffectParser(),
                    new PopulationEffectParser(),
                    new TaxModifierEffectParser(),
                    new UnitDefenseEffectParser(),
                    new UnitAttackEffectParser(),
                    new AddBuildingEffectParser(),
                    new IncreaseUnitAttackEffectParser(),
                    new BuildingCoralProductionEffectParser()
                }));
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestGetCountry(string username)
        {
            var info = await countryService.GetCountryInfoAsync(username);
            Assert.IsTrue(info.Pearls > 1000);
        }

        [TestMethod]
        public async Task TestGetAllCountries()
        {
            foreach (var u in context.Users)
            {
                await countryService.GetCountryInfoAsync(u.UserName);
            }
        }

        [TestMethod]
        public async Task TestRankList()
        {
            int index = 0;

            foreach (var cnt in context.Countries)
            {
                cnt.Rank = ++index;
            }

            var info = await countryService.GetRankedListAsync();
            Assert.IsTrue(info.Count() > 0);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            context.Users.Add(new Model.Entities.User
            { UserName = "poi" });
            await context.SaveChangesAsync();
            await countryService.CreateAsync("poi", "teszt");

            Assert.IsTrue(context.Countries.Any(c => c.Name == "teszt"));
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
