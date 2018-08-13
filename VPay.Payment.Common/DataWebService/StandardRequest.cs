using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class StandardRequest
    {
        public StandardRequest()
        {
            // TODO:  ugly stuff
        }

        public StandardRequest(
            CommonData commonData,
            CardData cardData,
            CheckData checkData,
            Claim claim,
            CorrespondenceData correspondenceData,
            CoveredItem coveredItem,
            Merchant merchant,
            Payment payment,
            SwitchTransaction switchTransaction)
        {
            CommonData = commonData;
            CardData = cardData;
            CheckData = checkData;
            Claim = claim;
            CorrespondenceData = correspondenceData;
            CoveredItem = coveredItem;
            Merchant = merchant;
            Payment = payment;
            SwitchTransaction = switchTransaction;
            Source = ' ';

            // TODO:  ugly stuff
        }

        public CommonData CommonData { get; set; }
        public CardData CardData { get; set; }
        public CheckData CheckData { get; set; }
        public Claim Claim { get; set; }
        public CorrespondenceData CorrespondenceData { get; set; }
        public CoveredItem CoveredItem { get; set; }
        public Merchant Merchant { get; set; }
        public Payment Payment { get; set; }
        public SwitchTransaction SwitchTransaction { get; set; }
        public char Source { get; set; } // Sourc Of Request P-webPage, S-webSvc

        public int LenCommon { get; set; }
        public int LenCard { get; set; }
        public int LenCheck { get; set; }
        public int LenClaim { get; set; }
        public int LenCorrsp { get; set; }
        public int LenCoverd { get; set; }
        public int LenMerchant { get; set; }
        public int LenPayment { get; set; }
        public int LenSwitch { get; set; }

        public int BufLen { get; set; }
        private int _bufMax = 4000;
    }
}
