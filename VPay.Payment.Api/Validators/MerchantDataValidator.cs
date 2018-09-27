using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class MerchantDataValidator : AbstractValidator<MerchantData>
    {
        public MerchantDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(MerchantData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.PayeeCode)
                .MaximumLength(propertyLengths[nameof(MerchantData.PayeeCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.PayeeName)
                .MaximumLength(propertyLengths[nameof(MerchantData.PayeeName)])
                .WithErrorCode("0990");

            RuleFor(x => x.ContactPerson)
                .MaximumLength(propertyLengths[nameof(MerchantData.ContactPerson)])
                .WithErrorCode("0990");

            RuleFor(x => x.PostalCode)
                .MaximumLength(propertyLengths[nameof(MerchantData.PostalCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.Telephone)
                .MaximumLength(propertyLengths[nameof(MerchantData.Telephone)])
                .WithErrorCode("0990");

            RuleFor(x => x.Fax)
                .MaximumLength(propertyLengths[nameof(MerchantData.Fax)])
                .WithErrorCode("0990");

            RuleFor(x => x.EmailAddress)
                .MaximumLength(propertyLengths[nameof(MerchantData.EmailAddress)])
                .WithErrorCode("0990");

        }

    }
}
