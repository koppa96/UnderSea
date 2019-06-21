using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Services.Researches;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Tests.Services
{
    [TestClass]
    public class ResearchServiceTests
    {
        private UnderSeaDatabaseContext context;
        private IResearchService researchService;

        [TestInitialize]
        public async Task Initialize()
        {
            context = await UtilityFactory.CreateContextAsync();
            researchService = new ResearchService(context, UtilityFactory.CreateMapper());
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        public async Task TestGetResearches(string username)
        {
            var researches = await researchService.GetResearchesAsync(username);

            var dbResearches = await context.CountryResearches.Include(cr => cr.Research)
                .Where(cr => cr.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            foreach (var researchInfo in researches)
            {
                var dbResearch = dbResearches.Single(cr => cr.Research.Id == researchInfo.Id);

                if (dbResearch != null)
                {
                    Assert.AreEqual(dbResearch.Count, researchInfo.Count);
                }
                else
                {
                    Assert.AreEqual(0, researchInfo.Count);
                }
            }
        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestStartResearch(string username)
        {

        }

        [TestMethod]
        [DataRow("ThePoor")]
        public async Task TestStartResearchNoMoney(string username)
        {

        }

        [TestMethod]
        [DataRow("TheRich")]
        public async Task TestStartResearchInProgress(string username)
        {

        }

        [TestMethod]
        [DataRow("TheResearcher")]
        public async Task TestStartResearchAlreadyFinished(string username)
        {

        }
    }
}
