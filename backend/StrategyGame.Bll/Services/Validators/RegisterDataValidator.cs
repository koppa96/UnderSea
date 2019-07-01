using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StrategyGame.Bll.Dto.Received.UserManagement;
using StrategyGame.Dal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrategyGame.Bll.Services.Validators
{
    public class RegisterDataValidator : AbstractValidator<RegisterData>
    {
        private readonly UnderSeaDatabaseContext _context;

        public RegisterDataValidator(UnderSeaDatabaseContext context)
        {
            _context = context;

            RuleFor(r => r.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(r => r.Username)
                .NotEmpty()
                .MustAsync(UsernameUniqueAsync)
                .WithMessage("Duplicate username.");

            RuleFor(r => r.Password)
                .MinimumLength(6)
                .Must(ValidPassword)
                .WithMessage("The password must contain a lowercase, an uppercase and a number.");

            RuleFor(r => r.CountryName)
                .Must(n => !string.IsNullOrWhiteSpace(n))
                .WithMessage("The country name must not be empty.");
        }

        public async Task<bool> UsernameUniqueAsync(string username, CancellationToken token = default)
        {
            return await _context.Users.AllAsync(u => u.UserName != username, token);
        }

        public static bool ValidPassword(string password)
        {
            var hasUpper = password.Any(c => char.IsUpper(c));
            var hasLower = password.Any(c => char.IsLower(c));
            var hasNumber = password.Any(c => char.IsDigit(c));

            return hasLower && hasNumber && hasUpper;
        }
    }
}
