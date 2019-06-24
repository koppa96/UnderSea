using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Sent;
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

        public UnitService(IMapper mapper, UnderSeaDatabaseContext database, ModifierParserContainer container)
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

        public async Task<UnitInfo> CreateUnitAsync(string username, int unitId, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException();
            }

            var globals = await Database.GlobalValues.SingleAsync();

            var unit = await Database.UnitTypes
                .Include(u => u.Content)
                .SingleOrDefaultAsync(u => u.Id == unitId);

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException();
            }

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

            // Check cost
            long costPearl = unit.CostPearl * count;
            long costCoral = unit.CostCoral * count;

            if (costPearl > country.Pearls || costCoral > country.Corals)
            {
                throw new InvalidOperationException("Units too expensive");
            }

            var builder = country.ParseAllEffectForCountry(globals, Parsers);
            var totalUnits = country.Commands.Sum(c => c.Divisions.Sum(d => d.Count));

            // Check pop-space
            if (builder.BarrackSpace < totalUnits + count)
            {
                throw new LimitReachedException();
            }

            var defenders = country.GetAllDefending();
            var targetDiv = defenders.Divisions.SingleOrDefault(d => d.Unit.Id == unitId);

            if (targetDiv == null)
            {
                Database.Divisions.Add(new Division
                { Count = count, ParentCommand = defenders, Unit = unit });
            }
            else
            {
                targetDiv.Count += count;
            }

            country.Pearls -= costPearl;
            country.Corals -= costCoral;

            await Database.SaveChangesAsync();

            var info = Mapper.Map<UnitType, UnitInfo>(unit);
            info.Count = count;
            return info;
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