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
              .ThenInclude(comm => comm.Divisons)
              .ThenInclude(d => d.Unit)
              .ThenInclude(u => u.Content)
              .SingleAsync(c => c.ParentUser.UserName == username).ConfigureAwait(false);

            var flattened = new Dictionary<UnitType, int>();
            foreach (var div in country.Commands.SelectMany(c => c.Divisons))
            {
                if (flattened.ContainsKey(div.Unit))
                {
                    flattened[div.Unit] += div.Count;
                }
                else
                {
                    flattened.Add(div.Unit, div.Count);
                }
            }

            return flattened.Select(d =>
            {
                var info = Mapper.Map<UnitType, UnitInfo>(d.Key);
                info.Count = d.Value;
                return info;
            });
        }

        public async Task<UnitInfo> CreateUnitAsync(string username, int unitId, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException();
            }

            var unit = await Database.UnitTypes.FindAsync(unitId).ConfigureAwait(false);

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            var country = await Database.Countries
               .Include(c => c.Commands)
               .ThenInclude(comm => comm.Divisons)
               .ThenInclude(d => d.Unit)
               .ThenInclude(u => u.Content)
               .SingleAsync(c => c.ParentUser.UserName == username).ConfigureAwait(false);

            // Check cost
            long costPearl = unit.CostPearl * count;
            long costCoral = unit.CostCoral * count;

            if (costPearl > country.Pearls || costCoral > country.Corals)
            {
                throw new InvalidOperationException("Units too expensive");
            }

            var builder = await Database.ParseAllEffectForCountry(country.Id, Parsers).ConfigureAwait(false);
            var totalUnits = country.Commands.Sum(c => c.Divisons.Sum(d => d.Count));

            // Check pop-space
            if (builder.BarrackSpace < totalUnits + count)
            {
                throw new LimitReachedException();
            }

            var defenders = country.GetAllDefending();
            var targetDiv = defenders.Divisons.SingleOrDefault(d => d.Unit.Id == unitId);

            if (targetDiv == null)
            {
                targetDiv = new Division() { Count = count, ParentCommand = defenders, Unit = unit };
                defenders.Divisons.Add(targetDiv);
            }
            else
            {
                targetDiv.Count += count;
            }

            country.Pearls -= costPearl;
            country.Corals -= costCoral;

            await Database.SaveChangesAsync().ConfigureAwait(false);

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

            var unit = await Database.UnitTypes.FindAsync(unitId).ConfigureAwait(false);

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            var country = await Database.Countries
               .Include(c => c.Commands)
               .ThenInclude(comm => comm.Divisons)
               .ThenInclude(d => d.Unit)
               .SingleAsync(c => c.ParentUser.UserName == username).ConfigureAwait(false);

            var defenders = country.GetAllDefending();
            var targetDiv = defenders.Divisons.SingleOrDefault(d => d.Unit.Id == unitId);

            if (targetDiv == null || targetDiv.Count < count)
            {
                throw new ArgumentException("Too many units deleted");
            }
            else
            {
                targetDiv.Count -= count;
            }

            await Database.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}