using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Country
{
    public class CountryService : ICountryService
    {
        protected IMapper Mapper { get; }
        protected UnderSeaDatabaseContext Context { get; }
        protected ModifierParserContainer Parsers { get; }
        protected AsyncReaderWriterLock TurnEndLock { get; }

        public CountryService(IMapper mapper, UnderSeaDatabaseContext context, AsyncReaderWriterLock turnEndLock,
            ModifierParserContainer parsers)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
            TurnEndLock = turnEndLock ?? throw new ArgumentNullException(nameof(turnEndLock));
        }

        public async Task CreateAsync(string username, string countryName, CancellationToken turnEndWaitToken)
        {
            using (var lck = await TurnEndLock.ReaderLockAsync(turnEndWaitToken))
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
                    Resources = (await Context.ResourceTypes.ToListAsync())
                        .Select(r => new CountryResource { Amount = r.StartingAmount, ResourceType = r }).ToList(),
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
               .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                            .ThenInclude(b => b.Effect)
               .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Content)
               .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(r => r.Effect)
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
               .Include(c => c.Resources)
                    .ThenInclude(r => r.ResourceType)
                        .ThenInclude(r => r.Content)
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
            info.Round = globals.Round;
            return await GatherInfoAsync(country, globals);
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
                infos.Add(await GatherInfoAsync(c, globals));
            }

            return infos;
        }

        protected async Task<CountryInfo> GatherInfoAsync(Model.Entities.Country country, GlobalValue globals)
        {
            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            info.Round = globals.Round;

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
                totalResearches[research.Research.Id] = Mapper.Map<CountryResearch, BriefCreationInfo>(research);
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

            var existingInfos = country.Commands.GetAllBriefUnitInfo(Mapper);

            // Add units not in the existing infos.
            info.ArmyInfo = existingInfos.Concat(await Context.UnitTypes
                .Include(u => u.Content)
                .Where(x => existingInfos.All(i => i.Id != x.Id))
                .Select(i => Mapper.Map<UnitType, BriefUnitInfo>(i)).ToListAsync())
                .ToList();

            info.UnseenReports = country.Attacks.Count(r => !r.IsSeenByAttacker) + country.Defenses.Count(r => !r.IsSeenByDefender);

            if (country.CurrentEvent != null)
            {
                info.Event = Mapper.Map<RandomEvent, EventInfo>(country.CurrentEvent);
            }

            var mods = country.ParseAllEffectForCountry(Context, globals, Parsers, false, false);
            var upkeep = country.GetTotalMaintenance();
            var perTurn = new Dictionary<ResourceType, long>();

            foreach (var res in country.Resources)
            {
                perTurn.Add(res.ResourceType, (long)Math.Round(0
                    + (mods.ResourceProductions.ContainsKey(res.ResourceType.Id) ? mods.ResourceProductions[res.ResourceType.Id] : 0
                        * (mods.ResourceModifiers.ContainsKey(res.ResourceType.Id) ? mods.ResourceModifiers[res.ResourceType.Id] : 1))
                    - (upkeep.ContainsKey(res.ResourceType.Id) ? upkeep[res.ResourceType.Id] : 0)));
            }

            info.ResourcesPerRound = perTurn.Select(r => new ResourceInfo
            {
                Name = r.Key.Content.Name,
                ImageUrl = r.Key.Content.ImageUrl,
                Amount = r.Value
            });

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

        public async Task<string> TransferAsync(string username, TransferDetails details)
        {
            var sender = await Context.Countries.Include(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == details.FromId);

            if (sender == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.FromId), "Invalid sender country id.");
            }

            if (sender.ParentUser.UserName != username)
            {
                throw new UnauthorizedAccessException("The sender must be your country.");
            }
            var receiver = await Context.Countries.Include(c => c.ParentUser)
                .SingleOrDefaultAsync(c => c.Id == details.ToId);

            if (receiver == null)
            {
                throw new ArgumentOutOfRangeException(nameof(details.ToId), "Invalid receiver country.");
            }

            foreach (var res in details.Resources)
            {
                var senderRes = sender.Resources.Single(r => r.Id == res.Id);
                var receiverRes = receiver.Resources.Single(r => r.Id == res.Id);

                if (senderRes.Amount < res.Amount)
                {
                    throw new ArgumentException("Not enough resource.");
                }

                senderRes.Amount -= res.Amount;
                receiverRes.Amount += res.Amount;
            }

            await Context.SaveChangesAsync();
            return receiver.ParentUser.UserName;
        }
    }
}