using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class Claim
    {
        public Claim()
        {
            UserKey = "";
            UserField1 = "";
            UserField2 = "";
            UserField3 = "";
            //Default to ISO 840 - USD
            CurrencyType = "840";
            Amount = "";
            ClaimDeductible = "";
            ClaimDate = "";
            ClaimOdometer = "";
            ClaimDescription = "";
            RequesterId = "";
            RequesterName = "";
            RepairOrderId = "";
            ClaimNotes = "";
            InitializeTotLen();
        }

        public Claim(string userKey, string userField1, string userField2, string userField3, string currencyType,
            string amount, string claimDeductible,
            string claimDate, string claimOdometer, string claimDescription, string requesterId, string requesterName,
            string repairOrderId, string claimNotes)
        {
            UserKey = userKey;
            UserField1 = userField1;
            UserField2 = userField2;
            UserField3 = userField3;
            CurrencyType = currencyType;
            if(string.IsNullOrEmpty(currencyType))
            {
                CurrencyType = "840";
            }
            Amount = amount;
            ClaimDeductible = claimDeductible;
            ClaimDate = claimDate;
            ClaimOdometer = claimOdometer;
            ClaimDescription = claimDescription;
            RequesterId = requesterId;
            RequesterName = requesterName;
            RepairOrderId = repairOrderId;
            ClaimNotes = claimNotes;
            InitializeTotLen();
        }

        public string UserKey { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string CurrencyType { get; set; }
        public string Amount { get; set; }
        public string ClaimDeductible { get; set; }
        public string ClaimDate { get; set; }
        public string ClaimOdometer { get; set; }
        public string ClaimDescription { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RepairOrderId { get; set; }
        public string ClaimNotes { get; set; }

        // Working Members Not Part Of Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        // member lengths
        private int _usrKeyLen = 100;
        private int _usrFd1Len = 20;
        private int _usrFd2Len = 20;
        private int _usrFd3Len = 20;
        private int _curTypLen = 3;
        private int _amountLen = 19;
        private int _clmDedLen = 8;
        private int _clmDteLen = 8;
        private int _clmOdoLen = 6;
        private int _clmDscLen = 40;
        private int _rqstIdLen = 20;
        private int _rqstNmLen = 50;
        private int _repOrdLen = 15;
        private int _clmNotLen = 120;

        private void InitializeTotLen()
        {
            TotLen = 10 +
                     _usrKeyLen + _usrFd1Len + _usrFd2Len + _usrFd3Len + _curTypLen + _amountLen +
                     _clmDedLen + _clmDteLen + _clmOdoLen + _clmDscLen + _rqstIdLen + _rqstNmLen +
                     _repOrdLen + _clmNotLen;
        }

        public override string ToString()
        {
            // TODO: Implement complex logic
            return "";
        }

        public bool IsValid()
        {
            bool returnValue = true;

            if (string.IsNullOrEmpty(UserKey) || string.IsNullOrEmpty(UserField1) || string.IsNullOrEmpty(Amount) || string.IsNullOrEmpty(ClaimDate))
                returnValue = false;

            return returnValue;
        }

        public bool IsEmpty()
        {
            string[] allRelevantStringsThatMightBeEmpty = new string[]
            {
                UserKey, UserField1, UserField2, UserField3, CurrencyType, Amount, ClaimDeductible, ClaimDate, ClaimOdometer, ClaimDescription,
                RequesterId, RequesterName, RepairOrderId, ClaimNotes
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

        public string ToISeriesString()
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("CLAIMDATA ".PadRight(10));
            builder.Append(UserKey.PadRight(_usrKeyLen));
            builder.Append(UserField1.PadRight(_usrFd1Len));
            builder.Append(UserField2.PadRight(_usrFd2Len));
            builder.Append(UserField3.PadRight(_usrFd3Len));
            builder.Append(CurrencyType.PadRight(_curTypLen));
            builder.Append(Amount.PadLeft(_amountLen));
            builder.Append(ClaimDeductible.PadLeft(_clmDedLen));
            builder.Append(ClaimDate.PadRight(_clmDteLen));
            builder.Append(ClaimOdometer.PadRight(_clmOdoLen));
            builder.Append(ClaimDescription.PadRight(_clmDscLen));
            builder.Append(RequesterId.PadRight(_rqstIdLen));
            builder.Append(RequesterName.PadRight(_rqstNmLen));
            builder.Append(RepairOrderId.PadRight(_repOrdLen));
            builder.Append(ClaimNotes.PadRight(_clmNotLen));

            return builder.ToString();
        }

        public void HydrateFromISeriesString(ref string iSeriesString)
        {
            int index = 10;
            if (iSeriesString.Substring(0, 10) != "CLAIMDATA ") return; // TODO: Handle weird errors better

            UserKey = ReadNext(ref iSeriesString, ref index, _usrKeyLen);
            UserField1 = ReadNext(ref iSeriesString, ref index, _usrFd1Len);
            UserField2 = ReadNext(ref iSeriesString, ref index, _usrFd2Len);
            UserField3 = ReadNext(ref iSeriesString, ref index, _usrFd3Len);
            CurrencyType = ReadNext(ref iSeriesString, ref index, _curTypLen);
            Amount = ReadNext(ref iSeriesString, ref index, _amountLen);
            ClaimDeductible = ReadNext(ref iSeriesString, ref index, _clmDedLen);
            ClaimDate = ReadNext(ref iSeriesString, ref index, _clmDteLen);
            ClaimOdometer = ReadNext(ref iSeriesString, ref index, _clmOdoLen);
            ClaimDescription = ReadNext(ref iSeriesString, ref index, _clmDscLen);
            RequesterId = ReadNext(ref iSeriesString, ref index, _rqstIdLen);
            RequesterName = ReadNext(ref iSeriesString, ref index, _rqstNmLen);
            RepairOrderId = ReadNext(ref iSeriesString, ref index, _repOrdLen);
            ClaimNotes = ReadNext(ref iSeriesString, ref index, _clmNotLen);
        }

        private string ReadNext(ref string inputString, ref int index, int length)
        {
            string rawValue = inputString.Substring(index, length);
            index += length;
            string returnValue = rawValue.Trim();

            return returnValue;
        }
    }
}
