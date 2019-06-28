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

        protected UnderSeaDatabaseContext Context { get; }

        protected ModifierParserContainer Parsers { get; }

        public UnitService(UnderSeaDatabaseContext context,
            ModifierParserContainer container, IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Parsers = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<IEnumerable<UnitInfo>> GetUnitInfoAsync()
        {
            return (await Context.UnitTypes
                .Include(u => u.Content)
                .ToListAsync())
                .Select(u => Mapper.Map<UnitType, UnitInfo>(u));
        }

        public async Task<IEnumerable<BriefUnitInfo>> CreateUnitAsync(string username, IEnumerable<PurchaseDetails> purchases)
        {
            var globals = await Context.GlobalValues.SingleAsync();

            var unitTypes = await Context.UnitTypes
                .Include(u => u.Content)
                .ToListAsync();

            var country = await Context.Countries
               .Include(c => c.Commands)
                    .ThenInclude(comm => comm.Divisions)
                        .ThenInclude(d => d.Unit)
                .Include(c => c.Buildings)
                    .ThenInclude(b => b.Building)
                        .ThenInclude(b => b.Effects)
                            .ThenInclude(bf => bf.Effect)
                .Include(c => c.Researches)
                    .ThenInclude(r => r.Research)
                        .ThenInclude(r => r.Effects)
                            .ThenInclude(rf => rf.Effect)
               .SingleAsync(c => c.ParentUser.UserName == username);

            var unitInfos = new List<BriefUnitInfo>();
            foreach (var purchase in purchases)
            {
                var unit = unitTypes.SingleOrDefault(u => u.Id == purchase.UnitId);

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

                var builder = country.ParseAllEffectForCountry(Context, globals, Parsers, false);
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
                    Context.Divisions.Add(targetDiv);
                }
                else
                {
                    targetDiv.Count += purchase.Count;
                }

                country.Pearls -= costPearl;
                country.Corals -= costCoral;

                var info = Mapper.Map<UnitType, BriefUnitInfo>(unit);
                info.Count = targetDiv.Count;
                unitInfos.Add(info);
            }

            await Context.SaveChangesAsync();
            return unitInfos;
        }

        public async Task DeleteUnitsAsync(string username, int unitId, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException();
            }

            var unit = await Context.UnitTypes.FindAsync(unitId);

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            var country = await Context.Countries
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

            await Context.SaveChangesAsync();
        }
    }
}