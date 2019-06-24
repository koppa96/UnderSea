using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Bll.Exceptions;

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

        public async Task<IEnumerable<CreationInfo>> GetResearchesAsync(string username)
        {
            var country = await _context.Countries
                .Include(c => c.Researches)
                    .ThenInclude(cr => cr.Research)
                        .ThenInclude(r => r.Content)
                .SingleAsync(c => c.ParentUser.UserName == username);

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

        public async Task StartResearchAsync(string username, int researchId)
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