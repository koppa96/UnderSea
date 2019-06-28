using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Dal;
using System.Linq;
using AutoMapper;
using StrategyGame.Model.Entities;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;

namespace StrategyGame.Bll.Services.Buildings
{
    public class BuildingService : IBuildingService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;

        public BuildingService(UnderSeaDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreationInfo>> GetBuildingsAsync(string username, int countryId)
        {
            var country = await _context.Countries
                .Include(c => c.Buildings)
                    .ThenInclude(cb => cb.Building)
                        .ThenInclude(b => b.Content)
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

            var creationInfos = country.Buildings.Select(cb => {
                var creationInfo = _mapper.Map<BuildingType, CreationInfo>(cb.Building);
                creationInfo.Count = cb.Count;
                return creationInfo;
            }).ToList();

            var buildingTypes = await _context.BuildingTypes.Include(b => b.Content).ToListAsync();
            foreach (var buildingInfo in buildingTypes)
            {
                if (creationInfos.Any(ci => ci.Id == buildingInfo.Id))
                {
                    continue;
                }

                creationInfos.Add(_mapper.Map<BuildingType, CreationInfo>(buildingInfo));
            }

            return creationInfos;
        }

        public async Task StartBuildingAsync(string username, int countryId, int buildingId)
        {
            var country = await _context.Countries.Include(c => c.InProgressBuildings)
                .Include(c => c.ParentUser)
                .Include(c => c.Resources)
                    .ThenInclude(r => r.ResourceType)
                .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "Invalid country id.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Not your country id.");
            }

            if (country.InProgressBuildings.Count > 0)
            {
                throw new InProgressException("The country already has a building in progress.");
            }

            var buildingType = await _context.BuildingTypes
                .Include(b => b.Cost)
                    .ThenInclude(c => c.ResourceType)
                        .ThenInclude(r => r.Content)
                .SingleOrDefaultAsync(b => b.Id == buildingId);

            if (buildingType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(buildingId), "No such building id.");
            }

            country.Purchase(buildingType, _context);

            var inProgressBuilding = new InProgressBuilding
            {
                ParentCountry = country,
                Building = buildingType,
                TimeLeft = KnownValues.DefaultBuildingTime
            };
            _context.InProgressBuildings.Add(inProgressBuilding);
            await _context.SaveChangesAsync();
        }
    }
}