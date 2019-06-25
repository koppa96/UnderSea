using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Services.Reports;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await reportService.SetSeenAsync(username, reportId);

            var combatInfos = await reportService.GetCombatInfoAsync(username);
            Assert.IsTrue(combatInfos.First().IsSeen);
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestMarkNotExistingReportSeen(string username)
        {
            await reportService.SetSeenAsync(username, -1);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task TestMarkOthersReportSeen(string username)
        {
            var reportId = (await context.Reports.FirstAsync(r => r.Attacker.ParentUser.UserName != username && r.Defender.ParentUser.UserName != username)).Id;
            await reportService.SetSeenAsync(username, reportId);
        }

        [TestMethod]
        [DataRow("TheResearcher", "TheBuilder")]
        public async Task TestDeleteReport(string attacker, string defender)
        {

        }

        [TestMethod]
        public async Task TestBothDeleteReport()
        {

        }

        [TestMethod]
        public async Task TestDeleteOthersReport()
        {

        }

        [TestMethod]
        public async Task TestDeleteNotExistingReport()
        {

        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
