using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class SwitchTransactionDataValidator : AbstractValidator<SwitchTransactionData>
    {
        public SwitchTransactionDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(SwitchTransactionData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.AcquireID)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.AcquireID)])
                .WithErrorCode("0990");

            RuleFor(x => x.AuthCode)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.AuthCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.AvailableBal)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.AvailableBal)])
                .WithErrorCode("0990");

            RuleFor(x => x.CaptureTS)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.CaptureTS)])
                .WithErrorCode("0990");

            RuleFor(x => x.CurrentBal)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.CurrentBal)])
                .WithErrorCode("0990");

            RuleFor(x => x.MerchantID)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.MerchantID)])
                .WithErrorCode("0990");

            RuleFor(x => x.OlsLogID)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.OlsLogID)])
                .WithErrorCode("0990");

            RuleFor(x => x.Stan)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.Stan)])
                .WithErrorCode("0990");

            RuleFor(x => x.Switch)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.Switch)])
                .WithErrorCode("0990");

            RuleFor(x => x.TerminalID)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.TerminalID)])
                .WithErrorCode("0990");

            RuleFor(x => x.TransactionTS)
                .MaximumLength(propertyLengths[nameof(SwitchTransactionData.TransactionTS)])
                .WithErrorCode("0990");

        }
    }
}
