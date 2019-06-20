using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;

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
                    .ThenInclude(b => b.Research)
                        .ThenInclude(b => b.Content)
               .SingleAsync(c => c.ParentUser.UserName == username).ConfigureAwait(false);

            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            info.Buildings = country.Buildings.Select(b => Mapper.Map<CountryBuilding, BriefCreationInfo>(b)).ToList();
            info.Researches = country.Researches.Select(r => Mapper.Map<CountryResearch, BriefCreationInfo>(r)).ToList();

            // Calculate the in-progress research count
            //foreach (var building in country.InProgressBuildings)

            throw new NotImplementedException();
        }

        public Task<IEnumerable<RankInfo>> GetRankedListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
