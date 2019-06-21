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

        public async Task<IEnumerable<CreationInfo>> GetBuildingsAsync(string username)
        {
            var country = await _context.Countries
                .Include(c => c.Buildings)
                    .ThenInclude(cb => cb.Building)
                        .ThenInclude(b => b.Content)
                .SingleAsync(c => c.ParentUser.UserName == username);

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

        public async Task StartBuildingAsync(string username, int buildingId)
        {
            var country = await _context.Countries.Include(c => c.InProgressBuildings)
                .SingleAsync(c => c.ParentUser.UserName == username);

            if (country.InProgressBuildings.Count > 0)
            {
                throw new InProgressException("The country already has a building in progress.");
            }

            var buildingType = await _context.BuildingTypes.FindAsync(buildingId);
            if (buildingType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(buildingId), "No such building id.");
            }

            if (buildingType.CostPearl > country.Pearls || buildingType.CostCoral > country.Corals)
            {
                throw new InvalidOperationException("Not enough money");
            }

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