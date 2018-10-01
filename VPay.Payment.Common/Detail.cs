using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class Detail
    {
        public Detail()
        {

        }

        public Detail(
            int tranId, int loadTran, string status,
            string amount, string authCode, string tranTimeStamp,
            string expiration, string merchantName, string merchantCode,
            string reasonCode, string reasonDesc, string actionCode,
            string actionDesc, string financialType, string requesterName,
            string batchNumber, string statusDesc)
        {
            TranId = tranId;
            LoadTran = loadTran;
            Status = status;
            Amount = amount;
            AuthCode = authCode;
            TranTimeStamp = tranTimeStamp;
            Expiration = expiration;
            MerchantCode = merchantCode;
            MerchantName = merchantName;
            ReasonCode = reasonCode;
            ReasonDesc = reasonDesc;
            ActionCode = actionCode;
            ActionDesc = actionDesc;
            FinancialType = financialType;
            RequesterName = requesterName;
            BatchNumber = batchNumber;
            StatusDesc = statusDesc;
        }

        public int TranId { get; set; }
        public int LoadTran { get; set; }
        public string Status { get; set; }
        public string Amount { get; set; } // M001
        public string AuthCode { get; set; }
        public string TranTimeStamp { get; set; }
        public string Expiration { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDesc { get; set; }
        public string ActionCode { get; set; }
        public string ActionDesc { get; set; }
        public string FinancialType { get; set; }
        public string RequesterName { get; set; }
        public string BatchNumber { get; set; }
        public string StatusDesc { get; set; }

    }
}
