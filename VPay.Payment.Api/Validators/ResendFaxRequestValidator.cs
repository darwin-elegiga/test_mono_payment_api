using FluentValidation;
using VPay.Payment.Api.Dtos;

namespace VPay.Payment.Api.Validators
{
    public class ResendFaxRequestValidator : AbstractValidator<ResendFaxRequest>
    {
        public ResendFaxRequestValidator()
        {
            // First set the cascade mode
            ClassLevelCascadeMode = CascadeMode.Continue;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FaxCode)
                .NotNull().WithErrorCode("0055").WithMessage("Invalid value for FaxCode");

            //RuleFor(x => x.CleanFaxNumber)
            //    .Matches("[0-9]{10,}").OverridePropertyName("FaxNumber").WithErrorCode("0005").WithMessage((f, s) => "New Fax Number Invalid: " + f.FaxNumber);
        }
    }
}
