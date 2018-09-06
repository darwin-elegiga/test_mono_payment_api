using FluentValidation;
using VPay.Payment.Api.Dtos;

namespace VPay.Payment.Api.Validators
{
    public class ChangeFaxNumberRequestValidator : AbstractValidator<ChangeFaxNumberRequest>
    {
        public ChangeFaxNumberRequestValidator()
        {
            // First set the cascade mode
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FaxCode)
                .NotNull().WithErrorCode("0055").WithMessage("Invalid value for FaxCode");

            RuleFor(x => x.CleanFaxNumber)
                .NotEmpty().OverridePropertyName("FaxNumber").WithErrorCode("0005").WithMessage("No Fax Number Provided")
                .Matches("[0-9]{10,}").OverridePropertyName("FaxNumber").WithErrorCode("0005").WithMessage((f, s) => "New Fax Number Invalid: " + f.FaxNumber);
        }
    }
}
