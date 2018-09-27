using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class CheckDataValidator : AbstractValidator<CheckData>
    {
        public CheckDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(CheckData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.CheckNumber)
                .MaximumLength(propertyLengths[nameof(CheckData.CheckNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.PosPayNumber)
                .MaximumLength(propertyLengths[nameof(CheckData.PosPayNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.SwitchNumber)
                .MaximumLength(propertyLengths[nameof(CheckData.SwitchNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.ChkNum1)
                .MaximumLength(propertyLengths[nameof(CheckData.ChkNum1)])
                .WithErrorCode("0990");

            RuleFor(x => x.ChkNum2)
                .MaximumLength(propertyLengths[nameof(CheckData.ChkNum2)])
                .WithErrorCode("0990");

            RuleFor(x => x.CheckDate)
                .MaximumLength(propertyLengths[nameof(CheckData.CheckDate)])
                .WithErrorCode("0990");

            RuleFor(x => x.Address1)
                .MaximumLength(propertyLengths[nameof(CheckData.Address1)])
                .WithErrorCode("0990");

            RuleFor(x => x.Address2)
                .MaximumLength(propertyLengths[nameof(CheckData.Address2)])
                .WithErrorCode("0990");

            RuleFor(x => x.Address3)
                .MaximumLength(propertyLengths[nameof(CheckData.Address3)])
                .WithErrorCode("0990");

            RuleFor(x => x.City)
                .MaximumLength(propertyLengths[nameof(CheckData.City)])
                .WithErrorCode("0990");

            RuleFor(x => x.StateOrProvince)
                .MaximumLength(propertyLengths[nameof(CheckData.StateOrProvince)])
                .WithErrorCode("0990");

            RuleFor(x => x.Zip)
                .MaximumLength(propertyLengths[nameof(CheckData.Zip)])
                .WithErrorCode("0990");

            RuleFor(x => x.County)
                .MaximumLength(propertyLengths[nameof(CheckData.County)])
                .WithErrorCode("0990");

            RuleFor(x => x.Region)
                .MaximumLength(propertyLengths[nameof(CheckData.Region)])
                .WithErrorCode("0990");

            RuleFor(x => x.Country)
                .MaximumLength(propertyLengths[nameof(CheckData.Country)])
                .WithErrorCode("0990");

            RuleFor(x => x.Memo)
                .MaximumLength(propertyLengths[nameof(CheckData.Memo)])
                .WithErrorCode("0990");
        }
    }
}
