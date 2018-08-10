using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class HeaderData
    {
        public HeaderData()
        {

        }

        public HeaderData(
            int transNumber, string client, string billCode,
            string billType, string availBalance, string currentBalance,
            string switchAvailBal, string switchCurrentBal, string payeeCode,
            string payeeName, string providerName, string requesterId,
            string requesterName, string taxId,
            string userField1, string userField2, string userField3)
        {
            TransNumber = transNumber;
            Client = client;
            BillCode = billCode;
            BillType = billType;
            AvailBalance = availBalance;
            CurrentBalance = currentBalance;
            SwitchAvailBal = switchAvailBal;
            SwitchCurrentBal = switchCurrentBal;
            PayeeCode = payeeCode;
            PayeeName = payeeName;
            ProviderName = providerName;
            RequesterId = requesterId;
            RequesterName = requesterName;
            TaxId = taxId;
            UserField1 = userField1;
            UserField2 = userField2;
            UserField3 = userField3;
        }

        public int TransNumber { get; set; }
        public string Client { get; set; }
        public string BillCode { get; set; }
        public string BillType { get; set; }
        public string AvailBalance { get; set; } // M001
        public string CurrentBalance { get; set; } // M001
        public string SwitchAvailBal { get; set; }
        public string SwitchCurrentBal { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeName { get; set; }
        public string ProviderName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string TaxId { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }

    }
}
