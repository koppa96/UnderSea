using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Country
{
    public class CountryService : ICountryService
    {
        protected IMapper Mapper { get; }

        protected UnderSeaDatabaseContext Database { get; }

        public CountryService(IMapper mapper, UnderSeaDatabaseContext database)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task CreateAsync(string username, string countryName)
        {
            var user = await Database.Users.SingleAsync(u => u.UserName == username);
            var globals = await Database.GlobalValues.SingleAsync();
            var country = new Model.Entities.Country()
            {
                Name = countryName,
                ParentUser = user,
                Corals = globals.StartingCorals,
                Pearls = globals.StartingPearls,
                Score = -1,
                Rank = -1
            };

            var defenders = new Command() { ParentCountry = country, TargetCountry = country };

            await Database.Countries.AddAsync(country);
            await Database.Commands.AddAsync(defenders);
            await Database.SaveChangesAsync();
        }

        public async Task<CountryInfo> GetCountryInfoAsync(string username)
        {
            var country = await Database.Countries
               .Include(c => c.Commands)
                    .ThenInclude(comm => comm.Divisons)
                        .ThenInclude(d => d.Unit)
                            .ThenInclude(u => u.Content)
               .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Content)
               .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Content)
               .Include(c => c.InProgressBuildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Content)
               .Include(c => c.InProgressResearches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Content)
               .SingleAsync(c => c.ParentUser.UserName == username);

            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            var globals = await Database.GlobalValues.SingleAsync();
            info.Round = globals.Round;

            // Start with all buildings and researches
            var totalBuildings = await Database.BuildingTypes.Include(r => r.Content)
                .ToDictionaryAsync(x => x.Id, x => Mapper.Map<BuildingType, BriefCreationInfo>(x));
            var totalResearches = await Database.ResearchTypes.Include(r => r.Content)
                .ToDictionaryAsync(x => x.Id, x => Mapper.Map<ResearchType, BriefCreationInfo>(x));

            // Map all existing buildings and researches
            foreach (var building in country.Buildings)
            {
                totalBuildings[building.Building.Id] = Mapper.Map<CountryBuilding, BriefCreationInfo>(building);
            }

            foreach (var research in country.Researches)
            {
                totalBuildings[research.Research.Id] = Mapper.Map<CountryResearch, BriefCreationInfo>(research);
            }

            // Add in progress buildings and researches
            foreach (var building in country.InProgressBuildings)
            {
                if (totalBuildings[building.Building.Id] == null)
                {
                    totalBuildings[building.Building.Id] = Mapper.Map<BuildingType, BriefCreationInfo>(building.Building);
                }
                else
                {
                    totalBuildings[building.Building.Id].InProgressCount++;
                }
            }

            foreach (var research in country.InProgressResearches)
            {
                if (totalResearches[research.Research.Id] == null)
                {
                    totalResearches[research.Research.Id] = Mapper.Map<ResearchType, BriefCreationInfo>(research.Research);
                }
                else
                {
                    totalResearches[research.Research.Id].InProgressCount++;
                }
            }

            info.Buildings = totalBuildings.Select(x => x.Value).ToList();
            info.Researches = totalResearches.Select(x => x.Value).ToList();
            info.ArmyInfo = await country.GetAllUnitInfoAsync(Database, Mapper);

            return info;
        }

        public async Task<IEnumerable<RankInfo>> GetRankedListAsync()
        {
            return await Database.Countries
                .Where(c => c.Rank > 0)
                .OrderBy(c => c.Rank)
                .Select(c => Mapper.Map<Model.Entities.Country, RankInfo>(c))
                .ToListAsync();
        }
    }
}
