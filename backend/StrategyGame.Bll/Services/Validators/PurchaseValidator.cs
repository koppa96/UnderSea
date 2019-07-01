using FluentValidation;
using FluentValidation.Results;
using StrategyGame.Bll.DTO.Received;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
