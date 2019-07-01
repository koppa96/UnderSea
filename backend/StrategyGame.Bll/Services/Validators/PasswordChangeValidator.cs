using FluentValidation;
using StrategyGame.Bll.Dto.Received.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Services.Validators
{
    public class PasswordChangeValidator : AbstractValidator<PasswordChangeData>
    {
        public PasswordChangeValidator()
        {
            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .Must(RegisterDataValidator.ValidPassword)
                .WithMessage("The password must contain a lowercase, an uppercase and a number.");
        }
    }
}
