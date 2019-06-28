using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Researches
{
    public class ResearchService : IResearchService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;

        public ResearchService(UnderSeaDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreationInfo>> GetResearchesAsync(string username, int countryId)
        {
            var country = await _context.Countries
                .Include(c => c.Researches)
                    .ThenInclude(cr => cr.Research)
                        .ThenInclude(r => r.Content)
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

            var creationInfos = country.Researches.Select(cr => {
                var creationInfo = _mapper.Map<ResearchType, CreationInfo>(cr.Research);
                creationInfo.Count = cr.Count;
                return creationInfo;
            }).ToList();

            var researchTypes = await _context.ResearchTypes.Include(r => r.Content).ToListAsync();
            foreach (var researchInfo in researchTypes)
            {
                if (creationInfos.Any(ci => ci.Id == researchInfo.Id))
                {
                    continue;
                }

                creationInfos.Add(_mapper.Map<ResearchType, CreationInfo>(researchInfo));
            }

            return creationInfos;
        }

        public async Task StartResearchAsync(string username, int countryId, int researchId)
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
                TimeLeft = KnownValues.DefaultResearchTime
            };

            _context.InProgressResearches.Add(inProgressResearch);
            await _context.SaveChangesAsync();
        }
    }
}