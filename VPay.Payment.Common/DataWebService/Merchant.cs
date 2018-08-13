using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class Merchant
    {
        public Merchant()
        {
            PayeeCode = "";
            PayeeName = "";
            ContactPerson = "";
            PostalCode = "";
            Telephone = "";
            Fax = "";
            EmailAddress = ""; // m005
            InitializeTotLen();
        }

        public Merchant(string payeeCode, string payeeName, string contactPerson,
            string postalCode, string telephone, string fax,
            string emailAddress)
        {
            PayeeCode = payeeCode;
            PayeeName = payeeName;
            ContactPerson = contactPerson;
            PostalCode = postalCode;
            Telephone = telephone;
            Fax = fax;
            EmailAddress = emailAddress; // m005
            InitializeTotLen();
        }

        public string PayeeCode { get; set; }
        public string PayeeName { get; set; }
        public string ContactPerson { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; } // M005

        // Working Variables Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        // member lengths
        private int _payCodLen = 10;
        private int _payNamLen = 30;
        private int _cntactLen = 30;
        private int _merZipLen = 15;
        private int _merPhnLen = 15;
        private int _merFaxLen = 15;
        private int _merEmlLen = 128; // m005

        private void InitializeTotLen()
        {
            TotLen = 10 + _payCodLen + _payNamLen + _cntactLen +
                _merZipLen + _merPhnLen + _merFaxLen +
                _merEmlLen; // m005
        }

        public override string ToString()
        {
            // TODO:  implement complex logic
            return "";
        }

        public bool IsValid()
        {
            bool returnValue = true;

            if (string.IsNullOrEmpty(PayeeCode) || string.IsNullOrEmpty(PayeeName) ||
                string.IsNullOrEmpty(ContactPerson) || string.IsNullOrEmpty(PostalCode) ||
                string.IsNullOrEmpty(Telephone) || string.IsNullOrEmpty(Fax))
            {
                returnValue = false;
            }

            return returnValue;
        }
    }
}
