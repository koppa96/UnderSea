using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
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
            var user = await Database.Users.SingleAsync(u => u.UserName == username).ConfigureAwait(false);
            var globals = await Database.GlobalValues.SingleAsync().ConfigureAwait(false);
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

            await Database.Countries.AddAsync(country).ConfigureAwait(false);
            await Database.Commands.AddAsync(defenders).ConfigureAwait(false);
            await Database.SaveChangesAsync().ConfigureAwait(false);
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
               .SingleAsync(c => c.ParentUser.UserName == username).ConfigureAwait(false);

            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            var totalBuildings = country.Buildings.ToDictionary(b => b.Building.Id, b => Mapper.Map<CountryBuilding, BriefCreationInfo>(b));
            var totalResearches = country.Researches.ToDictionary(r => r.Research.Id, r => Mapper.Map<CountryResearch, BriefCreationInfo>(r));

            // Calculate the in-progress research count
            var inProgressBuildings = country.InProgressBuildings.GroupBy(b => b.Building);

            foreach (var building in inProgressBuildings)
            {
                if (totalBuildings.ContainsKey(building.Key.Id))
                {
                    totalBuildings[building.Key.Id].InProgressCount += building.Count();
                }
                else
                {
                    var bInfo = Mapper.Map<BuildingType, BriefCreationInfo>(building.Key);
                    bInfo.InProgressCount += building.Count();
                    totalBuildings.Add(building.Key.Id, bInfo);
                }
            }

            var inProgressResearches = country.InProgressResearches.GroupBy(b => b.Research);

            foreach (var research in inProgressResearches)
            {
                if (totalResearches.ContainsKey(research.Key.Id))
                {
                    totalResearches[research.Key.Id].InProgressCount += research.Count();
                }
                else
                {
                    var rInfo = Mapper.Map<ResearchType, BriefCreationInfo>(research.Key);
                    rInfo.InProgressCount += research.Count();
                    totalResearches.Add(research.Key.Id, rInfo);
                }
            }

            info.Buildings = totalBuildings.Select(x => x.Value).ToList();
            info.Researches = totalResearches.Select(x => x.Value).ToList();

            return info;
        }

        public async Task<IEnumerable<RankInfo>> GetRankedListAsync()
        {
            return await Database.Countries
                .Where(c => c.Rank > 0)
                .OrderBy(c => c.Rank)
                .Select(c => Mapper.Map<Model.Entities.Country, RankInfo>(c))
                .ToListAsync().ConfigureAwait(false);
        }
    }
}
