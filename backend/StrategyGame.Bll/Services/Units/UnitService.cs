using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using StrategyGame.Bll.Dto.Sent;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Bll.EffectParsing;
using StrategyGame.Bll.Exceptions;
using StrategyGame.Bll.Extensions;
using StrategyGame.Dal;
using StrategyGame.Model.Entities;
using StrategyGame.Model.Entities.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Units
{
    public class UnitService : IUnitService
    {
        protected IMapper Mapper { get; }

        protected UnderSeaDatabaseContext Context { get; }

        protected ModifierParserContainer Parsers { get; }

        protected AsyncReaderWriterLock TurnEndLock { get; }

        public UnitService(UnderSeaDatabaseContext context, AsyncReaderWriterLock turnEndLock,
            ModifierParserContainer container, IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Parsers = container ?? throw new ArgumentNullException(nameof(container));
            TurnEndLock = turnEndLock ?? throw new ArgumentNullException(nameof(turnEndLock));
        }

        public async Task<IEnumerable<UnitInfo>> GetUnitInfoAsync()
        {
            return (await Context.UnitTypes.Include(u => u.Content).ToListAsync())
                .Select(u => Mapper.Map<UnitType, UnitInfo>(u));
        }

        public async Task<IEnumerable<BriefUnitInfo>> CreateUnitAsync(string username, int countryId,
            IEnumerable<PurchaseDetails> purchases, CancellationToken turnEndWaitToken)
        {
            using (var lck = await TurnEndLock.ReaderLockAsync(turnEndWaitToken))
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
                        .ThenInclude(b => b.Child)
                            .ThenInclude(b => b.Effects)
                                .ThenInclude(bf => bf.Child)
                    .Include(c => c.Researches)
                        .ThenInclude(r => r.Child)
                            .ThenInclude(r => r.Effects)
                   .SingleOrDefaultAsync(c => c.Id == countryId);

                if (country == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(countryId), "No country found by the provided ID.");
                }

                if (country.ParentUser.UserName != username)
                {
                    throw new UnauthorizedAccessException("Can't access country not owned by the user.");
                }

                var unitInfos = new List<BriefUnitInfo>();
                foreach (var purchase in purchases)
                {
                    var unit = unitTypes.Single(u => u.Id == purchase.UnitId);

                    if (unit == null)
                    {
                        throw new ArgumentOutOfRangeException(nameof(purchase.UnitId), "No unit found by the provided ID.");
                    }

                    if (!unit.IsPurchasable)
                    {
                        throw new InvalidOperationException("Can not purchase ranked up unit.");
                    }

                    if (purchase.Count < 0)
                    {
                        throw new ArgumentException("Purchase amount must be positive.");
                    }

                    var builder = country.ParseAllEffect(Context, globals, Parsers);
                    var totalUnits = country.Commands.Sum(c => c.Divisions.Sum(d => d.Count));

                    // Check pop-space
                    if (builder.BarrackSpace < totalUnits + purchase.Count)
                    {
                        throw new LimitReachedException("Unit count limit reached.");
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

                    country.Purchase(unit, purchase.Count);

                    var info = Mapper.Map<UnitType, BriefUnitInfo>(unit);
                    info.TotalCount = targetDiv.Count;
                    unitInfos.Add(info);
                }

                await Context.SaveChangesAsync();
                return unitInfos;
            }
        }

        public async Task DeleteUnitsAsync(string username, int countryId, int unitId, int count,
            CancellationToken turnEndWaitToken)
        {
            using (var lck = await TurnEndLock.ReaderLockAsync(turnEndWaitToken))
            {
                if (count < 1)
                {
                    throw new ArgumentException("Purchase amount must be positive.");
                }

                var unit = await Context.UnitTypes.FindAsync(unitId);

                if (unit == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(unitId), "No unit found by the provided ID.");
                }

                var country = await Context.Countries
                   .Include(c => c.Commands)
                   .ThenInclude(comm => comm.Divisions)
                   .ThenInclude(d => d.Unit)
                   .SingleOrDefaultAsync(c => c.Id == countryId);

                if (country == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(countryId), "No country found by the provided ID.");
                }

                if (country.ParentUser.UserName != username)
                {
                    throw new UnauthorizedAccessException("Can't access country not owned by the user.");
                }

                var defenders = country.GetAllDefending();
                var targetDiv = defenders.Divisions.SingleOrDefault(d => d.Unit.Id == unitId);

                if (targetDiv == null || targetDiv.Count < count)
                {
                    throw new ArgumentException("Count was bigger than the unit's total count.");
                }
                else
                {
                    targetDiv.Count -= count;
                }

                await Context.SaveChangesAsync();
            }
        }
    }
}