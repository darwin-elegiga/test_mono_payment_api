using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CorrespondenceData
    {
        public CorrespondenceData()
        {
            AttachmentLocation = "";
            DocumentID = "";
            Email = "";
            FaxCode = "";
            FaxStat = "";
            PhoneNumber = "";
            Type = "";
            InitializeTotLen();
        }

        public CorrespondenceData(string attachmentLocation, string documentID, string email,
            string faxCode, string faxStat, string phoneNumber, string type)
        {
            AttachmentLocation = attachmentLocation;
            DocumentID = documentID;
            Email = email;
            FaxCode = faxCode;
            FaxStat = faxStat;
            PhoneNumber = phoneNumber;
            Type = type;
            InitializeTotLen();
        }

        // VPay Data
        public string AttachmentLocation { get; set; }
        public string DocumentID { get; set; }
        public string Email { get; set; }
        public string FaxCode { get; set; }
        public string FaxStat { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }

        // Working Members Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        // field lengths
        private int _attLocLen = 256;
        private int _docIdLen = 20;
        private int _emailLen = 128;
        private int _faxCodLen = 10;    // HOLD / PENDING, etc, a fax code
        private int _faxStaLen = 10;    // From warranty ccsndFax value
        private int _phnNbrLen = 20;
        private int _typeLen = 20;

        private void InitializeTotLen()
        {
            TotLen = 10 + _attLocLen + _docIdLen + _emailLen
                + _faxCodLen + _faxStaLen + _phnNbrLen + _typeLen;
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
    }
}
