using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class CommonDataValidator : AbstractValidator<CommonData>
    {
        public CommonDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(CommonData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.TransNumber)
                .MaximumLength(propertyLengths[nameof(CommonData.TransNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.SeClaimID)
                .MaximumLength(propertyLengths[nameof(CommonData.SeClaimID)])
                .WithErrorCode("0990");

            RuleFor(x => x.TpaClaimID)
                .MaximumLength(propertyLengths[nameof(CommonData.TpaClaimID)])
                .WithErrorCode("0990");

            RuleFor(x => x.ProgramID)
                .MaximumLength(propertyLengths[nameof(CommonData.ProgramID)])
                .WithErrorCode("0990");

            RuleFor(x => x.ReasonCode)
                .MaximumLength(propertyLengths[nameof(CommonData.ReasonCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.ReasonDesc)
                .MaximumLength(propertyLengths[nameof(CommonData.ReasonDesc)])
                .WithErrorCode("0990");

            RuleFor(x => x.ResponseCode)
                .MaximumLength(propertyLengths[nameof(CommonData.ResponseCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.ResponseDesc)
                .MaximumLength(propertyLengths[nameof(CommonData.ResponseDesc)])
                .WithErrorCode("0990");

            RuleFor(x => x.SuccessCode)
                .MaximumLength(propertyLengths[nameof(CommonData.SuccessCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.SuccessDesc)
                .MaximumLength(propertyLengths[nameof(CommonData.SuccessDesc)])
                .WithErrorCode("0990");

            RuleFor(x => x.User)
                .MaximumLength(propertyLengths[nameof(CommonData.User)])
                .WithErrorCode("0990");

            RuleFor(x => x.PassWord)
                .MaximumLength(propertyLengths[nameof(CommonData.PassWord)])
                .WithErrorCode("0990");

            RuleFor(x => x.TimeStamp)
                .MaximumLength(propertyLengths[nameof(CommonData.TimeStamp)])
                .WithErrorCode("0990");

            RuleFor(x => x.Token)
                .MaximumLength(propertyLengths[nameof(CommonData.Token)])
                .WithErrorCode("0990");

        }
    }
}
