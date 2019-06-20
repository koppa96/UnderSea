using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;

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
            throw new System.NotImplementedException();
        }
    }
}