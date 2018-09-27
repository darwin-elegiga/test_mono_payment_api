using FluentValidation;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Validators
{
    public class StandardRequestValidator : AbstractValidator<StandardRequest>
    {
        public StandardRequestValidator()
        {
            RuleFor(x => x.CommonData).SetValidator(new CommonDataValidator());
            RuleFor(x => x.CardData).SetValidator(new CardDataValidator());
            RuleFor(x => x.CheckData).SetValidator(new CheckDataValidator());
            RuleFor(x => x.Claim).SetValidator(new ClaimDataValidator());
            RuleFor(x => x.CorrespondenceData).SetValidator(new CorrespondenceDataValidator());
            RuleFor(x => x.CoveredItem).SetValidator(new CoveredItemValidator());
            RuleFor(x => x.Merchant).SetValidator(new MerchantDataValidator());
            RuleFor(x => x.Payment).SetValidator(new PaymentDataValidator());
            RuleFor(x => x.SwitchTransaction).SetValidator(new SwitchTransactionValidator());

            RuleFor(x => x.Source).Must(x => x == 'P' || x == 'S').WithErrorCode("9999")
                .WithMessage("{PropertyName} must be either 'P' or 'S'");
        }
    }
}
