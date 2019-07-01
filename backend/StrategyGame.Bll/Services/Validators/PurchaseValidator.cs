using FluentValidation;
using StrategyGame.Bll.DTO.Received;
using System.Collections.Generic;

namespace StrategyGame.Bll.Services.Validators
{
    public class PurchaseValidator : AbstractValidator<IEnumerable<PurchaseDetails>>
    {
        public PurchaseValidator()
        {
            RuleForEach(p => p).Must(p => BeGreaterThanZero(p.Count)).WithErrorCode("Invalid amount.");
        }

        public bool BeGreaterThanZero(int amount)
        {
            return amount > 0;
        }
    }
}
