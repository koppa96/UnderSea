using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.EffectParsing;
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

        protected UnderSeaDatabaseContext Context { get; }

        protected ModifierParserContainer Parsers { get; }

        public CountryService(IMapper mapper, UnderSeaDatabaseContext context, ModifierParserContainer parsers)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        public async Task CreateAsync(string username, string countryName)
        {
            var user = await Context.Users
                .Include(u => u.RuledCountries)
                .SingleAsync(u => u.UserName == username);
            var globals = await Context.GlobalValues
                .Include(g => g.FirstStartingBuilding)
                .Include(g => g.SecondStartingBuilding)
                .SingleAsync();

            if (user.RuledCountries.Count > 0)
            {
                throw new InvalidOperationException("User already has a country");
            }

            var country = new Model.Entities.Country()
            {
                Name = countryName,
                ParentUser = user,
                Corals = globals.StartingCorals,
                Pearls = globals.StartingPearls,
                Score = -1,
                Rank = -1,
                CreatedRound = globals.Round
            };

            var defenders = new Command { ParentCountry = country, TargetCountry = country };

            Context.Countries.Add(country);
            Context.Commands.Add(defenders);
            Context.CountryBuildings.AddRange(
                new CountryBuilding { ParentCountry = country, Count = 1, Building = globals.FirstStartingBuilding },
                new CountryBuilding { ParentCountry = country, Count = 1, Building = globals.SecondStartingBuilding });

            await Context.SaveChangesAsync();
        }

        public async Task<CountryInfo> GetCountryInfoAsync(string username, int countryId)
        {
            var country = await Context.Countries
               .Include(c => c.Commands)
                    .ThenInclude(comm => comm.Divisions)
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
               .Include(c => c.CurrentEvent)
                    .ThenInclude(e => e.Content)
               .Include(c => c.Attacks)
               .Include(c => c.Defenses)
               .Include(c => c.ParentUser)
               .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "No country found by the provided ID.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't access country not owned by the user.");
            }

            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            var globals = await Context.GlobalValues.SingleAsync();
            var mods = country.ParseAllEffectForCountry(Context, globals, Parsers, false);

            info.Round = globals.Round;
            info.CoralsPerRound = (long)Math.Round(mods.CoralProduction * mods.HarvestModifier);
            info.PearlsPerRound = (long)Math.Round(mods.PearlProduction * mods.TaxModifier);

            return await GatherInfoAsync(country, globals.Round);
        }

        public async Task<IEnumerable<CountryInfo>> GetCountryInfoAsync(string username)
        {
            var countries = await Context.Countries
                  .Include(c => c.Commands)
                       .ThenInclude(comm => comm.Divisions)
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
                  .Include(c => c.CurrentEvent)
                       .ThenInclude(e => e.Content)
                  .Include(c => c.Attacks)
                  .Include(c => c.Defenses)
                  .Where(c => c.ParentUser.UserName == username).ToListAsync();

            var infos = new List<CountryInfo>(countries.Count);
            var globals = await Context.GlobalValues.SingleAsync();

            foreach (var c in countries)
            {
                infos.Add(await GatherInfoAsync(c, globals.Round));
            }

            return infos;
        }

        protected async Task<CountryInfo> GatherInfoAsync(Model.Entities.Country country, ulong round)
        {
            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            info.Round = round;

            // Start with all buildings and researches
            var totalBuildings = await Context.BuildingTypes.Include(r => r.Content)
                .ToDictionaryAsync(x => x.Id, x => Mapper.Map<BuildingType, BriefCreationInfo>(x));
            var totalResearches = await Context.ResearchTypes.Include(r => r.Content)
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

            info.ArmyInfo = (await country.GetAllUnitInfoAsync(Context, Mapper))
                .Select(ui => new BriefUnitInfo
                {
                    Id = ui.Id,
                    ImageUrl = ui.ImageUrl,
                    Count = ui.Count
                });

            info.UnseenReports = country.Attacks.Count(r => !r.IsSeenByAttacker) + country.Defenses.Count(r => !r.IsSeenByDefender);

            // Add event info
            if (country.CurrentEvent != null)
            {
                info.Event = Mapper.Map<RandomEvent, EventInfo>(country.CurrentEvent);
            }

            return info;
        }

        public async Task<IEnumerable<RankInfo>> GetRankedListAsync()
        {
            return await Context.Countries
                .Where(c => c.Rank > 0)
                .OrderBy(c => c.Rank)
                .Select(c => Mapper.Map<Model.Entities.Country, RankInfo>(c))
                .ToListAsync();
        }

        public async Task<IEnumerable<BriefCountryInfo>> GetCountriesAsync(string username)
        {
            return await Context.Countries
                .Where(c => c.ParentUser.UserName == username)
                .Select(c => new BriefCountryInfo { CountryId = c.Id, CountryName = c.Name })
                .ToListAsync();
        }

        public async Task<IEnumerable<CountryInfo>> GetAllCountryInfoAsync(string username)
        {
            var countries = await GetCountriesAsync(username);
            return await Task.WhenAll(countries.Select(c => GetCountryInfoAsync(username, c.CountryId)));
        }

        public async Task<BriefCountryInfo> BuyAsync(string username, int countryId, string countryName)
        {
            var country = await Context.Countries
                  .Include(c => c.ParentUser)
                  .SingleOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
            {
                throw new ArgumentOutOfRangeException(nameof(countryId), "No country found by the provided ID.");
            }

            if (country.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("Can't access country not owned by the user.");
            }

            var globals = await Context.GlobalValues
                .Include(g => g.FirstStartingBuilding)
                .Include(g => g.SecondStartingBuilding)
                .SingleAsync();

            if (globals.NewCountryCoralCost > country.Corals || globals.NewCountryPearlCost > country.Pearls)
            {
                throw new InvalidOperationException("Not enough resources to buy a new country.");
            }

            country.Corals -= globals.NewCountryCoralCost;
            country.Pearls -= globals.NewCountryPearlCost;

            var newCountry = new Model.Entities.Country()
            {
                Name = countryName,
                ParentUser = country.ParentUser,
                Corals = globals.StartingCorals,
                Pearls = globals.StartingPearls,
                Score = -1,
                Rank = -1,
                CreatedRound = globals.Round
            };

            var defenders = new Command { ParentCountry = newCountry, TargetCountry = newCountry };

            Context.Countries.Add(newCountry);
            Context.Commands.Add(defenders);
            Context.CountryBuildings.AddRange(
                new CountryBuilding { ParentCountry = newCountry, Count = 1, Building = globals.FirstStartingBuilding },
                new CountryBuilding { ParentCountry = newCountry, Count = 1, Building = globals.SecondStartingBuilding });

            await Context.SaveChangesAsync();

            return Mapper.Map<Model.Entities.Country, BriefCountryInfo>(newCountry);
        }
    }
}
