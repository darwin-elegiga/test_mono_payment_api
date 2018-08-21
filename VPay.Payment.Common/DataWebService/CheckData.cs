using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CheckData
    {
        public CheckData()
        {
            Number = "";
            PpChkNum = "";
            SwChkNum = "";
            ChkNum1 = "";
            ChkNum2 = "";
            Date = "";
            Address1 = "";
            Address2 = "";
            Address3 = "";
            City = "";
            StateOrProvince = "";
            Zip = "";
            County = "";
            Region = "";
            Country = "";
            EmailAddress = "";
            Memo = "";
            InitializeTotLen();
        }

        public CheckData(string number, string ppChkNum, string swChkNum,
            string chkNum1, string chkNum2, string date, string address1,
            string address2, string address3, string city,
            string stateOrProvince, string zip, string county, string region,
            string country, string emailAddress, string memo)
        {
            Number = number;
            PpChkNum = ppChkNum;
            SwChkNum = swChkNum;
            ChkNum1 = chkNum1;
            ChkNum2 = chkNum2;
            Date = date;
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
            City = city;
            StateOrProvince = stateOrProvince;
            Zip = zip;
            County = county;
            Region = region;
            Country = country;
            EmailAddress = emailAddress;
            Memo = memo;
            InitializeTotLen();
        }

        public string Number { get; set; }
        public string PpChkNum { get; set; }
        public string SwChkNum { get; set; }
        public string ChkNum1 { get; set; }
        public string ChkNum2 { get; set; }
        public string Date { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Zip { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public string Memo { get; set; }

        // Working Data Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }


        // member lengths
        private int _chkNumLen = 20;
        private int _cppNumLen = 20;
        private int _cswNumLen = 20;
        private int _chkNm1Len = 20;
        private int _chkNm2Len = 20;
        private int _chkDatLen = 8;
        private int _chkAd1Len = 50;
        private int _chkAd2Len = 50;
        private int _chkAd3Len = 50;
        private int _chkCtyLen = 20;
        private int _chkStpLen = 20;
        private int _chkZipLen = 24;
        private int _chkCntLen = 20;
        private int _chkRgnLen = 20;
        private int _chkCtrLen = 20;
        private int _chkEmlLen = 128;
        private int _chkMemLen = 100;

        private void InitializeTotLen()
        {
            TotLen = 10 +
                     _chkNumLen + _cppNumLen + _cswNumLen + _chkNm1Len + _chkNm2Len +
                     _chkDatLen + _chkAd1Len + _chkAd2Len + _chkAd3Len + _chkCtyLen +
                     _chkStpLen + _chkZipLen + _chkCntLen + _chkRgnLen + _chkCtrLen +
                     _chkMemLen; // m005
        }

        public override string ToString()
        {
            // TODO:  Implement complex logic
            return "";
        }

        public Boolean IsValid()
        {
            return true;
        }

        public bool IsEmpty()
        {
            string[] allRelevantStringsThatMightBeEmpty = new string[]
            {
                Number, PpChkNum, SwChkNum, ChkNum1, ChkNum2, Date, Address1, Address2, Address3, City, StateOrProvince, Zip,
                County, Region, Country, EmailAddress, Memo
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

            builder.Append("CHECKDATA ".PadRight(10));
            builder.Append(Number.PadRight(_chkNumLen));
            builder.Append(PpChkNum.PadRight(_cppNumLen));
            builder.Append(SwChkNum.PadRight(_cswNumLen));
            builder.Append(ChkNum1.PadRight(_chkNm1Len));
            builder.Append(ChkNum2.PadRight(_chkNm2Len));
            builder.Append(Date.PadRight(_chkDatLen));
            builder.Append(Address1.PadRight(_chkAd1Len));
            builder.Append(Address2.PadRight(_chkAd2Len));
            builder.Append(Address3.PadRight(_chkAd3Len));
            builder.Append(City.PadRight(_chkCtyLen));
            builder.Append(StateOrProvince.PadRight(_chkStpLen));
            builder.Append(Zip.PadRight(_chkZipLen));
            builder.Append(County.PadRight(_chkCntLen));
            builder.Append(Region.PadRight(_chkRgnLen));
            builder.Append(Country.PadRight(_chkCtrLen));
            // m005
            // builder.Append(EmailAddress.PadRight(_chkEmlLen));
            builder.Append(Memo.PadRight(_chkMemLen));


            return builder.ToString();
        }
    }
}
