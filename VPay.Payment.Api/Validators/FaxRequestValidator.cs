using FluentValidation;
using VPay.Payment.Api.Dtos;

namespace VPay.Payment.Api.Validators
{
    public class FaxRequestValidator : AbstractValidator<FaxRequest>
    {
        public FaxRequestValidator()
        {
            // First set the cascade mode
            ClassLevelCascadeMode = CascadeMode.Continue;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FaxCode)
                .NotNull().WithErrorCode("0055").WithMessage("Invalid value for FaxCode");
        }
    }
}
