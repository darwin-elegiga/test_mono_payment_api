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
            CommonSection = new CommonData();
            CardSection = new CardData();
            CheckSection = new CheckData();
            ClaimSection = new Claim();
            CorrespondenceSection = new CorrespondenceData();
            CoveredItemSection = new CoveredItem();
            MerchantSection = new Merchant();
            PaymentSection = new DataWebService.Payment();
            SwitchTransactionSection = new SwitchTransaction();
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

        private const int TotalParameterLength = 4000;

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

            while (builder.Length < TotalParameterLength - 2)
            {
                builder.Append(' ');
            }

            builder.Append("S ");

            string builderResult = builder.ToString();

            return builderResult;
        }

        public void PopulateDataFromISeriesResponse(string responseString)
        {
            int index = 0;
            string paddedString = responseString.PadRight(TotalLengthAll());

            string commonString = ReadNext(ref paddedString, ref index, CommonSection.TotLen);
            string cardString = ReadNext(ref paddedString, ref index, CardSection.TotLen);
            string checkString = ReadNext(ref paddedString, ref index, CheckSection.TotLen);
            string claimString = ReadNext(ref paddedString, ref index, ClaimSection.TotLen);
            string correspondenceString = ReadNext(ref paddedString, ref index, CorrespondenceSection.TotLen);
            string coveredItemString = ReadNext(ref paddedString, ref index, CoveredItemSection.TotLen);
            string merchantString = ReadNext(ref paddedString, ref index, MerchantSection.TotLen);
            string paymentString = ReadNext(ref paddedString, ref index, PaymentSection.TotLen);
            string switchTransactionString = ReadNext(ref paddedString, ref index, SwitchTransactionSection.TotLen);

            CommonSection.HydrateFromISeriesString(ref commonString);
            CardSection.HydrateFromISeriesString(ref cardString);
            CheckSection.HydrateFromISeriesString(ref checkString);
            ClaimSection.HydrateFromISeriesString(ref claimString);
            CorrespondenceSection.HydrateFromISeriesString(ref correspondenceString);
            CoveredItemSection.HydrateFromISeriesString(ref coveredItemString);
            MerchantSection.HydrateFromISeriesString(ref merchantString);
            PaymentSection.HydrateFromISeriesString(ref paymentString);
            SwitchTransactionSection.HydrateFromISeriesString(ref switchTransactionString);
        }

        private string ReadNext(ref string inputString, ref int index, int length)
        {
            string rawValue = inputString.Substring(index, length);
            index += length;
            string returnValue = rawValue;

            return returnValue;
        }

        private int TotalLengthAll()
        {
            int returnValue = CommonSection.TotLen + CardSection.TotLen + CheckSection.TotLen + ClaimSection.TotLen +
                              CorrespondenceSection.TotLen + CoveredItemSection.TotLen + MerchantSection.TotLen +
                              PaymentSection.TotLen + SwitchTransactionSection.TotLen;
            return returnValue;
        }
    }
}
