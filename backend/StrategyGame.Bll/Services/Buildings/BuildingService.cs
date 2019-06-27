using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CreationInfo>> GetBuildingsAsync()
        {
            return (await _context.BuildingTypes
                .Include(b => b.Content)
                .ToListAsync())
                .Select(b => _mapper.Map<BuildingType, CreationInfo>(b));
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

            country.Pearls -= buildingType.CostPearl;
            country.Corals -= buildingType.CostCoral;

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