using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class CorrespondenceDataValidator : AbstractValidator<CorrespondenceData>
    {
        public CorrespondenceDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(CorrespondenceData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.AttachmentLocation)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.AttachmentLocation)])
                .WithErrorCode("0990");

            RuleFor(x => x.DocumentID)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.DocumentID)])
                .WithErrorCode("0990");

            RuleFor(x => x.Email)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.Email)])
                .WithErrorCode("0990");

            RuleFor(x => x.FaxCode)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.FaxCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.FaxStat)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.FaxStat)])
                .WithErrorCode("0990");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.PhoneNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.Type)
                .MaximumLength(propertyLengths[nameof(CorrespondenceData.Type)])
                .WithErrorCode("0990");

            RuleSet("ChangeFaxNumber", () =>
            {
                RuleFor(x => x.FaxCode)
                    .NotNull().WithErrorCode("0055").WithMessage("Invalid value for FaxCode");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithErrorCode("0005").WithMessage("No Fax Number Provided")
                    .Must((x) => Regex.IsMatch(x.CleanFaxNumber(), "[0-9]{10,}")).WithErrorCode("0005")
                    .WithMessage((f, s) => "New Fax Number Invalid: " + f.PhoneNumber);
            });

            RuleSet("ResendFax", () =>
            {

                RuleFor(x => x.FaxCode)
                    .NotNull().WithErrorCode("0055").WithMessage("Invalid value for FaxCode");

                RuleFor(x => x.PhoneNumber)
                    .Must((x) => Regex.IsMatch(x.CleanFaxNumber(), "[0-9]{10,}")).WithErrorCode("0005")
                    .WithMessage((f, s) => "New Fax Number Invalid: " + f.PhoneNumber);
            });
        }
    }
}
