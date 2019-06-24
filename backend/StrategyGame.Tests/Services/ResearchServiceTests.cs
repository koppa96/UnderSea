﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Bll.Exceptions;
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
                var dbResearch = dbResearches.SingleOrDefault(cr => cr.Research.Id == researchInfo.Id);

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
            var researchId = (await context.ResearchTypes.FirstAsync()).Id;

            await researchService.StartResearchAsync(username, researchId);

            var inProgressResearches = await context.InProgressResearches
                .Include(r => r.Research)
                .Where(r => r.ParentCountry.ParentUser.UserName == username)
                .ToListAsync();

            Assert.AreEqual(1, inProgressResearches.Count);
            Assert.AreEqual(researchId, inProgressResearches.Single().Research.Id);
        }

        [TestMethod]
        [DataRow("ThePoor")]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestStartResearchNoMoney(string username)
        {
            var researchId = (await context.ResearchTypes.FirstAsync()).Id;

            await researchService.StartResearchAsync(username, researchId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(InProgressException))]
        public async Task TestStartResearchInProgress(string username)
        {
            var researchId = (await context.ResearchTypes.FirstAsync()).Id;

            await researchService.StartResearchAsync(username, researchId);
            await researchService.StartResearchAsync(username, researchId);
        }

        [TestMethod]
        [DataRow("TheResearcher")]
        [ExpectedException(typeof(LimitReachedException))]
        public async Task TestStartResearchAlreadyFinished(string username)
        {
            var researchId = (await context.CountryResearches
                .Include(r => r.Research)
                .Where(r => r.ParentCountry.ParentUser.UserName == username)
                .FirstAsync()).Research.Id;

            await researchService.StartResearchAsync(username, researchId);
        }

        [TestMethod]
        [DataRow("TheRich")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestStartResearchNotExisting(string username)
        {
            await researchService.StartResearchAsync(username, -1);
        }

        [TestCleanup]
        public void DisposeContext()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
