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

        public bool IsEmpty()
        {
            string[] allRelevantStringsThatMightBeEmpty = new string[]
            {
                PayeeCode, PayeeName, ContactPerson, PostalCode, Telephone, Fax, EmailAddress
            }; // EmailAddress is a maybe; ISeries mismatch

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

            builder.Append("MERCHANT  ".PadRight(10));
            builder.Append(PayeeCode.PadRight(_payCodLen));
            builder.Append(PayeeName.PadRight(_payNamLen));
            builder.Append(ContactPerson.PadRight(_cntactLen));
            builder.Append(PostalCode.PadRight(_merZipLen));
            builder.Append(Telephone.PadRight(_merPhnLen));
            builder.Append(Fax.PadRight(_merFaxLen));
            builder.Append(EmailAddress.PadRight(_merEmlLen));   // m005

            return builder.ToString();
        }

        public void HydrateFromISeriesString(ref string iSeriesString)
        {
            int index = 10;
            if (iSeriesString.Substring(0, 10) != "MERCHANT  ") return; // TODO: Handle weird errors better

            PayeeCode = ReadNext(ref iSeriesString, ref index, _payCodLen);
            PayeeName = ReadNext(ref iSeriesString, ref index, _payNamLen);
            ContactPerson = ReadNext(ref iSeriesString, ref index, _cntactLen);
            PostalCode = ReadNext(ref iSeriesString, ref index, _merZipLen);
            Telephone = ReadNext(ref iSeriesString, ref index, _merPhnLen);
            Fax = ReadNext(ref iSeriesString, ref index, _merFaxLen);
            EmailAddress = ReadNext(ref iSeriesString, ref index, _merEmlLen);   // m005
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
