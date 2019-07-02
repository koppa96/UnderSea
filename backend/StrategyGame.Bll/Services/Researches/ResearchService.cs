using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Researches
{
    public class ResearchService : IResearchService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly AsyncReaderWriterLock _turnEndLock;

        public ResearchService(UnderSeaDatabaseContext context, AsyncReaderWriterLock turnEndLock, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _turnEndLock = turnEndLock;
        }

        public async Task<IEnumerable<CreationInfo>> GetResearchesAsync()
        {
            return (await _context.ResearchTypes
                .Include(r => r.Content)
                .ToListAsync())
                .Select(r => _mapper.Map<ResearchType, CreationInfo>(r));
        }

        public async Task StartResearchAsync(string username, int researchId, CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
            {
                var country = await _context.Countries.Include(c => c.InProgressResearches)
                    .Include(c => c.Researches)
                        .ThenInclude(cr => cr.Research)
                    .SingleAsync(c => c.ParentUser.UserName == username);

                if (country.InProgressResearches.Count > 0)
                {
                    throw new InProgressException("The country already has a research in progress.");
                }

                var research = country.Researches.SingleOrDefault(cr => cr.Research.Id == researchId);
                if (research != null && research.Count >= research.Research.MaxCompletedAmount)
                {
                    throw new LimitReachedException("The max research count has been reached.");
                }

                var researchType = await _context.ResearchTypes.FindAsync(researchId);
                if (researchType == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(researchId), "No such research id.");
                }

                if (country.Pearls < researchType.CostPearl || country.Corals < researchType.CostCoral)
                {
                    throw new InvalidOperationException("Not enough money.");
                }

                country.Pearls -= researchType.CostPearl;
                country.Corals -= researchType.CostCoral;

                var inProgressResearch = new InProgressResearch
                {
                    ParentCountry = country,
                    Research = researchType,
                    TimeLeft = KnownValues.DefaultResearchTime
                };

                _context.InProgressResearches.Add(inProgressResearch);
                await _context.SaveChangesAsync();
            }
        }
    }
}