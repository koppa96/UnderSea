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

namespace StrategyGame.Bll.Services.Buildings
{
    public class BuildingService : IBuildingService
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly AsyncReaderWriterLock _turnEndLock;

        public BuildingService(UnderSeaDatabaseContext context, AsyncReaderWriterLock turnEndLock, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _turnEndLock = turnEndLock;
        }

        public async Task<IEnumerable<CreationInfo>> GetBuildingsAsync()
        {
            return (await _context.BuildingTypes
                .Include(b => b.Content)
                .ToListAsync())
                .Select(b => _mapper.Map<BuildingType, CreationInfo>(b));
        }

        public async Task<IEnumerable<BriefCreationInfo>> GetBuildingsAsync(string username, int countryId)
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

            var creationInfos = country.Buildings.Select(cb =>
            {
                var creationInfo = _mapper.Map<BuildingType, BriefCreationInfo>(cb.Building);
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

                creationInfos.Add(_mapper.Map<BuildingType, BriefCreationInfo>(buildingInfo));
            }

            return creationInfos;
        }

        public async Task StartBuildingAsync(string username, int countryId, int buildingId, CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
            {
                var country = await _context.Countries.Include(c => c.InProgressBuildings)
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

                if (country.InProgressBuildings.Any(b => b.Building.Id == buildingId))
                {
                    throw new InProgressException("There is already a building of this type in progress.");
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
}