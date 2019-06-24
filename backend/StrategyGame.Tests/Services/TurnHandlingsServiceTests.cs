using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Services.TurnHandling;
using StrategyGame.Dal;
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
            turnService = new TurnHandlingService(new ModifierParserContainer(new AbstractEffectModifierParser[]
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
        public async Task TestTurnHandling()
        {
            var currentTurn = (await context.GlobalValues.SingleAsync()).Round;
            await turnService.EndTurnAsync(context);

            // TODO: More comprehensive testing

            Assert.AreEqual(currentTurn + 1, (await context.GlobalValues.SingleAsync()).Round);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}