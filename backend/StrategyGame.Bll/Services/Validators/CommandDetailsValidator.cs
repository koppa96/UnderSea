using FluentValidation;
using StrategyGame.Bll.Dto.Received;
using StrategyGame.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Extensions;

namespace StrategyGame.Bll.Services.Validators
{
    public class CommandDetailsValidator : AbstractValidator<CommandDetails>
    {
        private readonly UnderSeaDatabaseContext _context;
        private readonly string _user;

        public CommandDetailsValidator(UnderSeaDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext.User.Identity.Name;
            _context = context;

            RuleFor(d => d.TargetCountryId).MustAsync(TargetCountryExistAsync)
                .WithMessage("Invalid target country id.");

            RuleFor(d => d.Units).MustAsync(UnitIdsValidAsync)
                .WithMessage("Invalid unit id.");

            RuleFor(d => d.Units).MustAsync(UnitDetailsValidAsync)
                .WithMessage("Not enough units");
        }

        public async Task<bool> TargetCountryExistAsync(int countryId, CancellationToken ct = default)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId && c.Id != countryId, ct);
        }

        public async Task<bool> UnitIdsValidAsync(IEnumerable<UnitDetails> details, CancellationToken ct = default)
        {
            var unitTypes = await _context.UnitTypes.ToListAsync(ct);

            return details.Select(d => new
            {
                detail = d, unitType = unitTypes.SingleOrDefault(u => u.Id == d.UnitId)
            }).All(x => x.unitType != null);
        }

        public async Task<bool> UnitDetailsValidAsync(IEnumerable<UnitDetails> details, CancellationToken ct = default)
        {
            var country = await _context.Countries.Include(c => c.Commands)
                    .ThenInclude(c => c.Divisions)
                        .ThenInclude(c => c.Unit)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.ParentCountry)
                .Include(c => c.Commands)
                    .ThenInclude(c => c.TargetCountry)
                .SingleAsync(c => c.ParentUser.UserName == _user, ct);

            var defenders = country.GetAllDefending();
            return details.Select(detail => new
            {
                detail, division = defenders.Divisions.SingleOrDefault(div => div.Unit.Id == detail.UnitId)
            }).All(x => x.division != null && x.division.Count > x.detail.Amount);
        }
    }
}
