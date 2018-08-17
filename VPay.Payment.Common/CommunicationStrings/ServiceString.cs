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

            if (!CommonSection.IsEmpty())
            {
                builder.Append("CommonData [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!CardSection.IsEmpty())
            {
                builder.Append("CardData [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!CheckSection.IsEmpty())
            {
                builder.Append("CheckData [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!ClaimSection.IsEmpty())
            {
                builder.Append("Claim [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!CorrespondenceSection.IsEmpty())
            {
                builder.Append("CorrespondenceData [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!CoveredItemSection.IsEmpty())
            {
                builder.Append("Covered Item [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!MerchantSection.IsEmpty())
            {
                builder.Append("Merchant [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!PaymentSection.IsEmpty())
            {
                builder.Append("Payment [ ");
                // etc
                builder.Append(" ]\n");
            }

            if (!SwitchTransactionSection.IsEmpty())
            {
                builder.Append("SwitchTransaction [ ");
                // etc
                builder.Append(" ]\n");
            }

            return builder.ToString();
        }
    }
}
