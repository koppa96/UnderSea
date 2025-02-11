﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.Dto.Sent.Country;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
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
                var user = await Context.Users.SingleAsync(u => u.UserName == username);
                var globals = await Context.GlobalValues
                    .Include(g => g.FirstStartingBuilding)
                    .Include(g => g.SecondStartingBuilding)
                    .SingleAsync();

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
        }

        public async Task<CountryInfo> GetCountryInfoAsync(string username)
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
                            .ThenInclude(bf => bf.Effect)
               .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Content)
               .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(rf => rf.Effect)
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
               .SingleAsync(c => c.ParentUser.UserName == username);

            var info = Mapper.Map<Model.Entities.Country, CountryInfo>(country);
            var globals = await Context.GlobalValues.SingleAsync();
            var mods = country.ParseAllEffectForCountry(Context, globals, Parsers, false, false);
            var (pearlUpkeep, coralUpkeep) = country.GetTotalMaintenance();

            info.Round = globals.Round;
            info.CoralsPerRound = (long)Math.Round(mods.CoralProduction * mods.HarvestModifier - coralUpkeep);
            info.PearlsPerRound = (long)Math.Round(mods.Population * globals.BaseTaxation * mods.TaxModifier
                + mods.PearlProduction - pearlUpkeep);
            info.BarrackSpace = mods.BarrackSpace;

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

            info.UnseenReports = country.Attacks.Count(r => !r.IsSeenByAttacker && !r.IsDeletedByAttacker) +
                                 country.Defenses.Count(r => !r.IsSeenByDefender && !r.IsDeletedByDefender);

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
    }
}
