using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class CoveredItemDataValidator : AbstractValidator<CoveredItemData>
    {
        public CoveredItemDataValidator()
        {

            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(CoveredItemData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }

            RuleFor(x => x.ItemType)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.ItemType)])
                .WithErrorCode("0990");

            RuleFor(x => x.ItemId)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.ItemId)])
                .WithErrorCode("0990");

            RuleFor(x => x.ItemYear)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.ItemYear)])
                .WithErrorCode("0990");

            RuleFor(x => x.Manufacturer)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.Manufacturer)])
                .WithErrorCode("0990");

            RuleFor(x => x.Model)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.Model)])
                .WithErrorCode("0990");

            RuleFor(x => x.BookStateOrProvince)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.BookStateOrProvince)])
                .WithErrorCode("0990");

            RuleFor(x => x.PostalCode)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.PostalCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.PlanCode)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.PlanCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.PlanDescription)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.PlanDescription)])
                .WithErrorCode("0990");

            RuleFor(x => x.Deductible)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.Deductible)])
                .WithErrorCode("0990");

            RuleFor(x => x.NewUsed)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.NewUsed)])
                .WithErrorCode("0990");

            RuleFor(x => x.BeginDate)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.BeginDate)])
                .WithErrorCode("0990");

            RuleFor(x => x.ExpireDate)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.ExpireDate)])
                .WithErrorCode("0990");

            RuleFor(x => x.OdometerType)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.OdometerType)])
                .WithErrorCode("0990");

            RuleFor(x => x.BeginOdometer)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.BeginOdometer)])
                .WithErrorCode("0990");

            RuleFor(x => x.ExpireOdometer)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.ExpireOdometer)])
                .WithErrorCode("0990");

            RuleFor(x => x.OwnerLastName)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.OwnerLastName)])
                .WithErrorCode("0990");

            RuleFor(x => x.OwnerFirstName)
                .MaximumLength(propertyLengths[nameof(CoveredItemData.OwnerFirstName)])
                .WithErrorCode("0990");

        }
    }
}
