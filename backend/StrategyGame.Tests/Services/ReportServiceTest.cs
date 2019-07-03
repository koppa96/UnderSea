using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Services.Reports;
using StrategyGame.Dal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class ReportServiceTest
    {
        private UnderSeaDatabaseContext context;
        private IReportService reportService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            reportService = new ReportService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        public async Task TestGetReportInfo(string username)
        {
            var combatInfos = await reportService.GetCombatInfoAsync(username);

            Assert.AreEqual(1, combatInfos.Count());
            Assert.IsTrue(combatInfos.First().IsAttack);
            Assert.IsTrue(combatInfos.First().IsWon);
            Assert.IsFalse(combatInfos.First().IsSeen);
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        public async Task TestMarkReportSeen(string username)
        {
            var reportId = (await context.Reports.FirstAsync(r => r.Attacker.ParentUser.UserName == username)).Id;
            await reportService.SetCombatReportSeenAsync(username, reportId);

            var combatInfos = await reportService.GetCombatInfoAsync(username);
            Assert.IsTrue(combatInfos.First().IsSeen);
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestMarkNotExistingReportSeen(string username)
        {
            await reportService.SetCombatReportSeenAsync(username, -1);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task TestMarkOthersReportSeen(string username)
        {
            var reportId = (await context.Reports.FirstAsync(r => r.Attacker.ParentUser.UserName != username && r.Defender.ParentUser.UserName != username)).Id;
            await reportService.SetCombatReportSeenAsync(username, reportId);
        }

        [TestMethod]
        [DataRow("TheResearcher", "TheBuilder")]
        public async Task TestDeleteReport(string attacker, string defender)
        {
            var reportId = (await context.Reports.FirstAsync()).Id;

            await reportService.DeleteCombatReportAsync(attacker, reportId);

            Assert.AreEqual(0, (await reportService.GetCombatInfoAsync(attacker)).Count());
            Assert.AreEqual(1, (await reportService.GetCombatInfoAsync(defender)).Count());
        }

        [TestMethod]
        [DataRow("TheResearcher", "TheBuilder")]
        public async Task TestBothDeleteReport(string attacker, string defender)
        {
            var reportId = (await context.Reports.FirstAsync()).Id;

            await reportService.DeleteCombatReportAsync(attacker, reportId);
            await reportService.DeleteCombatReportAsync(defender, reportId);

            Assert.AreEqual(0, (await reportService.GetCombatInfoAsync(defender)).Count());
            Assert.AreEqual(0, await context.Reports.CountAsync());
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task TestDeleteOthersReport(string username)
        {
            var reportId = (await context.Reports.FirstAsync()).Id;

            await reportService.DeleteCombatReportAsync(username, reportId);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestDeleteNotExistingReport(string username)
        {
            await reportService.DeleteCombatReportAsync(username, -1);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
