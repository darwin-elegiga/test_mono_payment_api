using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class ClaimDataValidator : AbstractValidator<ClaimData>
    {
        public ClaimDataValidator()
        {
            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(ClaimData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.UserKey)
                .MaximumLength(propertyLengths[nameof(ClaimData.UserKey)])
                .WithErrorCode("0990");

            RuleFor(x => x.UserField1)
                .MaximumLength(propertyLengths[nameof(ClaimData.UserField1)])
                .WithErrorCode("0990");

            RuleFor(x => x.UserField2)
                .MaximumLength(propertyLengths[nameof(ClaimData.UserField2)])
                .WithErrorCode("0990");

            RuleFor(x => x.UserField3)
                .MaximumLength(propertyLengths[nameof(ClaimData.UserField3)])
                .WithErrorCode("0990");

            RuleFor(x => x.CurrencyType)
                .MaximumLength(propertyLengths[nameof(ClaimData.CurrencyType)])
                .WithErrorCode("0990");

            RuleFor(x => x.Amount)
                .MaximumLength(propertyLengths[nameof(ClaimData.Amount)])
                .WithErrorCode("0990");

            RuleFor(x => x.ClaimDeductible)
                .MaximumLength(propertyLengths[nameof(ClaimData.ClaimDeductible)])
                .WithErrorCode("0990");

            RuleFor(x => x.ClaimDate)
                .MaximumLength(propertyLengths[nameof(ClaimData.ClaimDate)])
                .WithErrorCode("0990");

            RuleFor(x => x.ClaimOdometer)
                .MaximumLength(propertyLengths[nameof(ClaimData.ClaimOdometer)])
                .WithErrorCode("0990");

            RuleFor(x => x.ClaimDescription)
                .MaximumLength(propertyLengths[nameof(ClaimData.ClaimDescription)])
                .WithErrorCode("0990");

            RuleFor(x => x.RequesterId)
                .MaximumLength(propertyLengths[nameof(ClaimData.RequesterId)])
                .WithErrorCode("0990");

            RuleFor(x => x.RequesterName)
                .MaximumLength(propertyLengths[nameof(ClaimData.RequesterName)])
                .WithErrorCode("0990");

            RuleFor(x => x.RepairOrderId)
                .MaximumLength(propertyLengths[nameof(ClaimData.RepairOrderId)])
                .WithErrorCode("0990");

            RuleFor(x => x.ClaimNotes)
                .MaximumLength(propertyLengths[nameof(ClaimData.ClaimNotes)])
                .WithErrorCode("0990");

        }
    }
}
