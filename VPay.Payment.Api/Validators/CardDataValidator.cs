using System.Collections.Generic;
using FluentValidation;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class CardDataValidator : AbstractValidator<CardData>
    {


        public CardDataValidator()
        {
            var propertyLengths = new Dictionary<string, int>();
            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(CardData)))
            {
                propertyLengths.Add(prop.Name, attr.Length);
            }


            RuleFor(x => x.CardType)
                .MaximumLength(propertyLengths[nameof(CardData.CardType)])
                .WithErrorCode("0990");

            RuleFor(x => x.CardNumber)
                .MaximumLength(propertyLengths[nameof(CardData.CardNumber)])
                .WithErrorCode("0990");

            RuleFor(x => x.CardCvv2)
                .MaximumLength(propertyLengths[nameof(CardData.CardCvv2)])
                .WithErrorCode("0990");

            RuleFor(x => x.CardExpiration)
                .MaximumLength(propertyLengths[nameof(CardData.CardExpiration)])
                .WithErrorCode("0990");

            RuleFor(x => x.LoadTransId)
                .MaximumLength(propertyLengths[nameof(CardData.LoadTransId)])
                .WithErrorCode("0990");

            RuleFor(x => x.LoadAmount)
                .MaximumLength(propertyLengths[nameof(CardData.LoadAmount)])
                .WithErrorCode("0990");

            RuleFor(x => x.LoadFee)
                .MaximumLength(propertyLengths[nameof(CardData.LoadFee)])
                .WithErrorCode("0990");

            RuleFor(x => x.CardholderName)
                .MaximumLength(propertyLengths[nameof(CardData.CardholderName)])
                .WithErrorCode("0990");

            RuleFor(x => x.CardholderAddress)
                .MaximumLength(propertyLengths[nameof(CardData.CardholderAddress)])
                .WithErrorCode("0990");

            RuleFor(x => x.UnloadCode)
                .MaximumLength(propertyLengths[nameof(CardData.UnloadCode)])
                .WithErrorCode("0990");

            RuleFor(x => x.UnloadDesc)
                .MaximumLength(propertyLengths[nameof(CardData.UnloadDesc)])
                .WithErrorCode("0990");

            RuleFor(x => x.VcRef)
                .MaximumLength(propertyLengths[nameof(CardData.VcRef)])
                .WithErrorCode("0990");

            RuleFor(x => x.DisplayCVV2)
                .MaximumLength(propertyLengths[nameof(CardData.DisplayCVV2)])
                .WithErrorCode("0990");

            RuleFor(x => x.MaskPan)
                .MaximumLength(propertyLengths[nameof(CardData.MaskPan)])
                .WithErrorCode("0990");

        }
    }
}
