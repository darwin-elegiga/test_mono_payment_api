using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class Payment
    {
        public Payment()
        {
            AcctngCode = "";
            AcctngDesc = "";
            AccountNumber = "";
            Action = "";
            AvailableBalance = "";
            BillCode = "";
            Client = "";
            CurrencyCode = "";
            CurrentBalance = "";
            Free = "";
            FutureUse = "";
            Id = "";
            LoadAmount = "";
            PanNumber = "";
            RequestedAmount = "";
            RoutingNumber = "";
            Type = "";
            InitializeTotLen();
        }

        public Payment(string acctngCode, string acctngDesc, string accountNumber, string action,
            string availableBalance, string billCode, string client, string currencyCode, string currentBalance,
            string free, string futureUse, string id, string loadAmount, string panNumber, string requestedAmount,
            string routingNumber, string type)
        {
            AcctngCode = acctngCode;
            AcctngDesc = acctngDesc;
            AccountNumber = accountNumber;
            Action = action;
            AvailableBalance = availableBalance;
            BillCode = billCode;
            Client = client;
            CurrencyCode = currencyCode;
            CurrentBalance = currentBalance;
            Free = free;
            FutureUse = futureUse;
            Id = id;
            LoadAmount = loadAmount;
            PanNumber = panNumber;
            RequestedAmount = requestedAmount;
            RoutingNumber = routingNumber;
            Type = type;
            InitializeTotLen();
        }

        // VPay Data
        public string AcctngCode { get; set; }
        public string AcctngDesc { get; set; }
        public string AccountNumber { get; set; }
        public string Action { get; set; }
        public string AvailableBalance { get; set; }
        public string BillCode { get; set; }
        public string Client { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrentBalance { get; set; }
        public string Free { get; set; }
        public string FutureUse { get; set; }
        public string Id { get; set; }
        public string LoadAmount { get; set; }
        public string PanNumber { get; set; }
        public string RequestedAmount { get; set; }
        public string RoutingNumber { get; set; }
        public string Type { get; set; }

        // Working Members Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int TotLen { get; set; }
        public int StrIdx { get; set; }
        public int EndIdx { get; set; }

        // field Lengths
        private int _actCodLen = 1;
        private int _actDscLen = 7;
        private int _actNbrLen = 24;
        private int _actionLen = 20;
        private int _avlBalLen = 20;
        private int _billCdLen = 20;
        private int _clientLen = 10;
        private int _curCodLen = 10;
        private int _curBalLen = 20;
        private int _freeLen = 20;
        private int _futUseLen = 2;
        private int _idLen = 20;
        private int _lodAmtLen = 20;
        private int _panNbrLen = 32;
        private int _reqAmtLen = 20;
        private int _rtgNbrLen = 24;
        private int _typeLen = 10;

        private void InitializeTotLen()
        {
            TotLen = 10 +  // 10 spaces for the section header PAYMENT
                     _actCodLen + _actDscLen +
                     _actNbrLen + _actionLen + _avlBalLen + _billCdLen + _clientLen +
                     _curCodLen + _curBalLen + _freeLen + _futUseLen + _idLen +
                     _lodAmtLen + _panNbrLen + _reqAmtLen + _rtgNbrLen + _typeLen;
        }

        public override string ToString()
        {
            // TODO:  implement complex logic
            return "";
        }

        public bool IsValid()
        {
            return true;
        }

        public bool IsEmpty()
        {
            string[] allRelevantStringsThatMightBeEmpty = new string[]
            {
                AcctngCode, AcctngDesc, AccountNumber, Action, AvailableBalance, BillCode, Client, CurrencyCode, CurrentBalance, Free, FutureUse,
                Id, LoadAmount, PanNumber, RequestedAmount, RoutingNumber, Type
            };

            bool isEverythingEmpty = true;
            foreach (string nextString in allRelevantStringsThatMightBeEmpty)
            {
                if (!string.IsNullOrEmpty(nextString))
                {
                    isEverythingEmpty = false;
                    break;
                }
            }

            return isEverythingEmpty;
        }
    }
}
