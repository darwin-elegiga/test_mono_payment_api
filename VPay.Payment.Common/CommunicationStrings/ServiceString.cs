using System;
using System.Collections.Generic;
using System.Text;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Common.CommunicationStrings
{
    public class ServiceString
    {
        public ServiceString()
        {

        }

        public ServiceString(CommonData commonData, CardData cardData, CheckData checkData, Claim claim,
            CorrespondenceData correspondenceData, CoveredItem coveredItem, Merchant merchant,
            DataWebService.Payment payment, SwitchTransaction switchTransaction)
        {
            CommonSection = commonData;
            CardSection = cardData;
            CheckSection = checkData;
            ClaimSection = claim;
            CorrespondenceSection = correspondenceData;
            CoveredItemSection = coveredItem;
            MerchantSection = merchant;
            PaymentSection = payment;
            SwitchTransactionSection = switchTransaction;
        }

        public CommonData CommonSection { get; set; }
        public CardData CardSection { get; set; }
        public CheckData CheckSection { get; set; }
        public Claim ClaimSection { get; set; }
        public CorrespondenceData CorrespondenceSection { get; set; }
        public CoveredItem CoveredItemSection { get; set; }
        public Merchant MerchantSection { get; set; }
        public DataWebService.Payment PaymentSection { get; set; }
        public SwitchTransaction SwitchTransactionSection { get; set; }

        public string GenerateStringForISeriesCall()
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append(CommonSection.ToISeriesString());
            builder.Append(CardSection.ToISeriesString());
            builder.Append(CheckSection.ToISeriesString());
            builder.Append(ClaimSection.ToISeriesString());
            builder.Append(CorrespondenceSection.ToISeriesString());
            builder.Append(CoveredItemSection.ToISeriesString());
            builder.Append(MerchantSection.ToISeriesString());
            builder.Append(PaymentSection.ToISeriesString());
            builder.Append(SwitchTransactionSection.ToISeriesString());

            return builder.ToString();
        }
    }
}
