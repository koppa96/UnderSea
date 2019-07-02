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

        public async Task StartBuildingAsync(string username, int countryId, int buildingId, CancellationToken turnEndWaitToken)
        {
            using (var lck = await _turnEndLock.ReaderLockAsync(turnEndWaitToken))
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

                if (country.InProgressBuildings.Any(b => b.Building.Id == buildingId))
                {
                    throw new InProgressException("There is already a building of this type in progress.");
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
                    TimeLeft = buildingType.BuildTime
                };
                _context.InProgressBuildings.Add(inProgressBuilding);
                await _context.SaveChangesAsync();
            }
        }
    }
}