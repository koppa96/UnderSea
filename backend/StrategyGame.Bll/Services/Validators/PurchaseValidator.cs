using FluentValidation;
using Microsoft.AspNetCore.Http;
using StrategyGame.Bll.DTO.Received;
using StrategyGame.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace StrategyGame.Bll.Services.Validators
{
    // List because IEnumerable doesn't work (It should)
    public class PurchaseValidator : AbstractValidator<List<PurchaseDetails>>
    {
        private readonly string _user;
        private readonly UnderSeaDatabaseContext _context;

        public PurchaseValidator(IHttpContextAccessor contextAccessor, UnderSeaDatabaseContext context)
        {
            _user = contextAccessor.HttpContext.User.Identity.Name;
            _context = context;

            RuleFor(p => p).MustAsync(HaveEnoughMoneyAsync)
                .WithMessage("Not enough money.");

            RuleFor(p => p)
                .Must(pl => pl.All(p => p.Count > 0))
                .WithMessage("Invalid amount.");
                
            RuleFor(p => p).MustAsync(UnitIdsExistAsync)
                .WithMessage("Invalid unit id.");
        }

        public async Task<bool> UnitIdsExistAsync(List<PurchaseDetails> details, CancellationToken token = default)
        {
            var unitIds = await _context.UnitTypes.Select(u => u.Id).ToListAsync();

            foreach (var detail in details)
            {
                if (!unitIds.Any(u => u == detail.UnitId))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> HaveEnoughMoneyAsync(List<PurchaseDetails> details, CancellationToken token = default)
        {
            var unitTypes = await _context.UnitTypes.ToListAsync();
            var country = await _context.Countries.SingleAsync(c => c.ParentUser.UserName == _user);

            throw new System.NotImplementedException();
            //int cost = details.Sum(d => d.Count * unitTypes.SingleOrDefault(u => u.Id == d.UnitId)?.CostPearl ?? 0);

            //return cost <= country.Pearls;
        }
    }
}
