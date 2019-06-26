using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Units
{
    public class UnitService : IUnitService
    {
        protected IMapper Mapper { get; }

        protected UnderSeaDatabaseContext Database { get; }

        protected ModifierParserContainer Parsers { get; }

        public UnitService(UnderSeaDatabaseContext database,
            ModifierParserContainer container, IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Parsers = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<IEnumerable<UnitInfo>> GetUnitInfoAsync(string username)
        {
            var country = await Database.Countries
              .Include(c => c.Commands)
              .ThenInclude(comm => comm.Divisions)
              .ThenInclude(d => d.Unit)
              .ThenInclude(u => u.Content)
              .SingleAsync(c => c.ParentUser.UserName == username);

            return await country.GetAllUnitInfoAsync(Database, Mapper);
        }

        public async Task<IEnumerable<UnitInfo>> CreateUnitAsync(string username, IEnumerable<PurchaseDetails> purchases)
        {
            var globals = await Database.GlobalValues.SingleAsync();

            var unitTypes = await Database.UnitTypes
                .Include(u => u.Content)
                .ToListAsync();

            var country = await Database.Countries
               .Include(c => c.Commands)
                    .ThenInclude(comm => comm.Divisions)
                        .ThenInclude(d => d.Unit)
                .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
               .SingleAsync(c => c.ParentUser.UserName == username);

            var unitInfos = new List<UnitInfo>();
            foreach (var purchase in purchases)
            {
                var unit = unitTypes.Single(u => u.Id == purchase.UnitId);

                if (unit == null)
                {
                    throw new ArgumentOutOfRangeException("Invalid unit id.");
                }

                if (purchase.Count < 0)
                {
                    throw new ArgumentException("Invalid amount.");
                }
                
                // Check cost
                long costPearl = unit.CostPearl * purchase.Count;
                long costCoral = unit.CostCoral * purchase.Count;

                if (costPearl > country.Pearls || costCoral > country.Corals)
                {
                    throw new InvalidOperationException("Units too expensive");
                }

                var builder = country.ParseAllEffectForCountry(Database, globals, Parsers, false);
                var totalUnits = country.Commands.Sum(c => c.Divisions.Sum(d => d.Count));

                // Check pop-space
                if (builder.BarrackSpace < totalUnits + purchase.Count)
                {
                    throw new LimitReachedException();
                }

                var defenders = country.GetAllDefending();
                var targetDiv = defenders.Divisions.SingleOrDefault(d => d.Unit.Id == purchase.UnitId);

                if (targetDiv == null)
                {
                    targetDiv = new Division { Count = purchase.Count, ParentCommand = defenders, Unit = unit };
                    Database.Divisions.Add(targetDiv);
                }
                else
                {
                    targetDiv.Count += purchase.Count;
                }

                country.Pearls -= costPearl;
                country.Corals -= costCoral;

                var info = Mapper.Map<UnitType, UnitInfo>(unit);
                info.Count = targetDiv.Count;
                unitInfos.Add(info);
            }

            await Database.SaveChangesAsync();
            return unitInfos;
        }

        public async Task DeleteUnitsAsync(string username, int unitId, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException();
            }

            var unit = await Database.UnitTypes.FindAsync(unitId);

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            var country = await Database.Countries
               .Include(c => c.Commands)
               .ThenInclude(comm => comm.Divisions)
               .ThenInclude(d => d.Unit)
               .SingleAsync(c => c.ParentUser.UserName == username);

            var defenders = country.GetAllDefending();
            var targetDiv = defenders.Divisions.SingleOrDefault(d => d.Unit.Id == unitId);

            if (targetDiv == null || targetDiv.Count < count)
            {
                throw new ArgumentException("Too many units deleted");
            }
            else
            {
                targetDiv.Count -= count;
            }

            await Database.SaveChangesAsync();
        }
    }
}