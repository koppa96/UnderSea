using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;
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

        public async Task StartResearchAsync(string username, int countryId, int researchId, CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
            {
                var country = await _context.Countries.Include(c => c.InProgressResearches)
                                .Include(c => c.Researches)
                                    .ThenInclude(cr => cr.Research)
                                .Include(c => c.ParentUser)
                                .SingleOrDefaultAsync(c => c.Id == countryId);

                if (country == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid country id.");
                }

                if (country.ParentUser.UserName != username)
                {
                    throw new UnauthorizedAccessException("Not your country id.");
                }

                if (country.InProgressResearches.Count > 0)
                {
                    throw new InProgressException("The country already has a research in progress.");
                }

                var research = country.Researches.SingleOrDefault(cr => cr.Research.Id == researchId);
                if (research != null && research.Count >= research.Research.MaxCompletedAmount)
                {
                    throw new LimitReachedException("The max research count has been reached.");
                }

                var researchType = await _context.ResearchTypes
                    .Include(b => b.Cost)
                        .ThenInclude(c => c.ResourceType)
                            .ThenInclude(r => r.Content)
                    .SingleOrDefaultAsync(b => b.Id == researchId);

                if (researchType == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(researchId), "No such research id.");
                }

                country.Purchase(researchType, _context);

                var inProgressResearch = new InProgressResearch
                {
                    ParentCountry = country,
                    Research = researchType,
                    TimeLeft = researchType.ResearchTime
                };

                _context.InProgressResearches.Add(inProgressResearch);
                await _context.SaveChangesAsync();
            }
        }
    }
}