using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class PaymentDataValidator : AbstractValidator<PaymentData>
    {
        public PaymentDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(PaymentData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.AccountingCode)
                .MaximumLength(propertyLengths[nameof(PaymentData.AccountingCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.AccountingDesc)
                .MaximumLength(propertyLengths[nameof(PaymentData.AccountingDesc)])
                .WithErrorCode("0990");

            RuleFor(x => x.AccountNumber)
                .MaximumLength(propertyLengths[nameof(PaymentData.AccountNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.AvailableBalance)
                .MaximumLength(propertyLengths[nameof(PaymentData.AvailableBalance)])
                .WithErrorCode("0990");

            RuleFor(x => x.BillCode)
                .MaximumLength(propertyLengths[nameof(PaymentData.BillCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.Client)
                .MaximumLength(propertyLengths[nameof(PaymentData.Client)])
                .WithErrorCode("0990");

            RuleFor(x => x.CurrencyCode)
                .MaximumLength(propertyLengths[nameof(PaymentData.CurrencyCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.CurrentBalance)
                .MaximumLength(propertyLengths[nameof(PaymentData.CurrentBalance)])
                .WithErrorCode("0990");

            RuleFor(x => x.Free)
                .MaximumLength(propertyLengths[nameof(PaymentData.Free)])
                .WithErrorCode("0990");

            RuleFor(x => x.FutureUse)
                .MaximumLength(propertyLengths[nameof(PaymentData.FutureUse)])
                .WithErrorCode("0990");

            RuleFor(x => x.Id)
                .MaximumLength(propertyLengths[nameof(PaymentData.Id)])
                .WithErrorCode("0990");

            RuleFor(x => x.LoadAmount)
                .MaximumLength(propertyLengths[nameof(PaymentData.LoadAmount)])
                .WithErrorCode("0990");

            RuleFor(x => x.PanNumber)
                .MaximumLength(propertyLengths[nameof(PaymentData.PanNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.RequestedAmount)
                .MaximumLength(propertyLengths[nameof(PaymentData.RequestedAmount)])
                .WithErrorCode("0990");

            RuleFor(x => x.RoutingNumber)
                .MaximumLength(propertyLengths[nameof(PaymentData.RoutingNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.Type)
                .MaximumLength(propertyLengths[nameof(PaymentData.Type)])
                .WithErrorCode("0990");

        }
    }
}
