using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class TurnHandlingsServiceTests
    {
        private UnderSeaDatabaseContext context;
        private ITurnHandlingService turnService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            turnService = new TurnHandlingService(ModifierParserContainer.CreateDefault());
        }

        [TestMethod]
        public async Task TestTurnHandling()
        {
            var currentTurn = (await context.GlobalValues.SingleAsync()).Round;
            await turnService.EndTurnAsync(context);
            Assert.AreEqual(currentTurn + 1, (await context.GlobalValues.SingleAsync()).Round);
        }

        [TestMethod]
        [DataRow("TheCommander")]
        public async Task TestRandomEvent(string username)
        {
            var globals = await context.GlobalValues.SingleAsync();
            globals.Round = 50;
            globals.RandomEventChance = 1.0;

            var country = await context.Countries
                .Include(c => c.ParentUser)
                .SingleAsync(x => x.ParentUser.UserName == username);

            await turnService.EndTurnAsync(context);

            globals.Round = 50;
            globals.RandomEventChance = 0.0;
            await turnService.EndTurnAsync(context);

            Assert.AreEqual(country.CurrentEvent, null);
        }

        [TestMethod]
        [DataRow("TheCommander")]
        public async Task TestBuildingComplete(string username)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Buildings)
                .Include(c => c.InProgressBuildings)
                .SingleAsync(x => x.ParentUser.UserName == username);
            var buildingCount = country.Buildings.Count;
            var inProgressCount = country.InProgressBuildings.Count;

            await turnService.EndTurnAsync(context);

            Assert.AreEqual(buildingCount + 1, country.Buildings.Count);
            Assert.AreEqual(inProgressCount - 1, country.InProgressBuildings.Count);
        }

        [TestMethod]
        [DataRow("TheCommander")]
        public async Task TestResearchComplete(string username)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Researches)
                .Include(c => c.InProgressResearches)
                .SingleAsync(x => x.ParentUser.UserName == username);
            var researchCount = country.Researches.Count;
            var inProgressCount = country.InProgressResearches.Count;

            await turnService.EndTurnAsync(context);

            Assert.AreEqual(researchCount + 1, country.Researches.Count);
            Assert.AreEqual(inProgressCount - 1, country.InProgressResearches.Count);
        }

        [TestMethod]
        [DataRow("TheCommander", "ThePoor")]
        public async Task TestDefenseLoss(string username, string username2)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                .Include(c => c.Attacks)
                .Include(c => c.Defenses)
                .SingleAsync(x => x.ParentUser.UserName == username);
            var poorCountry = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                .Include(c => c.Attacks)
                .Include(c => c.Defenses)
                .SingleAsync(x => x.ParentUser.UserName == username2);
            poorCountry.Corals = 50000;
            poorCountry.Pearls = 50000;

            var unitCount = country.Commands.Sum(c => c.Divisions.Sum(d => d.Count));

            Assert.IsFalse(poorCountry.Attacks.Any());
            Assert.IsFalse(country.Defenses.Any());

            await turnService.EndTurnAsync(context);

            Assert.IsTrue(poorCountry.Attacks.Any());
            Assert.IsTrue(country.Defenses.Any());
            Assert.IsTrue(poorCountry.Attacks.First().DidAttackerWin);

            Assert.IsTrue(unitCount > country.Commands.Sum(c => c.Divisions.Sum(d => d.Count)));
        }

        [TestMethod]
        [DataRow("ThePoor")]
        public async Task TestDesertingUnits(string username)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                .SingleAsync(x => x.ParentUser.UserName == username);

            Assert.IsTrue(country.Commands.Sum(c =>
                c.Divisions.Sum(d => d.Count)) > 0);

            await turnService.EndTurnAsync(context);

            Assert.AreEqual(country.Commands.Sum(c =>
               c.Divisions.Sum(d => d.Count)), 0);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        public async Task TestPartialDesertingUnits(string username)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                .SingleAsync(x => x.ParentUser.UserName == username);

            country.Corals = 500;
            country.Pearls = 500;
            var unitCount = country.Commands.Sum(c =>
                c.Divisions.Sum(d => d.Count));

            Assert.IsTrue(unitCount > 0);

            await turnService.EndTurnAsync(context);

            Assert.IsTrue(unitCount > country.Commands.Sum(c =>
                c.Divisions.Sum(d => d.Count)));
            Assert.IsTrue(country.Corals >= 0);
            Assert.IsTrue(country.Pearls >= 0);
            Assert.IsTrue(country.Pearls < 500);
            Assert.IsTrue(country.Pearls < 500);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        public async Task TestLooting(string username)
        {
            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(d => d.Unit)
                .SingleAsync(x => x.ParentUser.UserName == username);

            var coralMaintenance = country.Commands.Sum(c =>
                c.Divisions.Sum(d => d.Count * d.Unit.MaintenanceCoral));
            var pearlMaintenance = country.Commands.Sum(c =>
                c.Divisions.Sum(d => d.Count * d.Unit.MaintenancePearl));

            country.Corals = 50000;
            country.Pearls = 50000;

            await turnService.EndTurnAsync(context);

            Assert.IsTrue(country.Corals > 50000 - coralMaintenance);
            Assert.IsTrue(country.Pearls > 50000 - pearlMaintenance);
        }


        [TestMethod]
        [DataRow("ThePoor")]
        public async Task TestAllEffect(string username)
        {
            var magicBuilding = new BuildingType
            { Effects = context.Effects.Select(x => new BuildingEffect { Effect = x }).ToList() };
            context.BuildingTypes.Add(magicBuilding);

            var country = await context.Countries
                .Include(c => c.ParentUser)
                .Include(c => c.Buildings)
                .Include(c => c.InProgressBuildings)
                .SingleAsync(x => x.ParentUser.UserName == username);

            country.Buildings.Add(new CountryBuilding { Building = magicBuilding, Count = 1 });

            await turnService.EndTurnAsync(context);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}