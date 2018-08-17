using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CommonData
    {
        public CommonData()
        {
            TransNumber = "";
            SeClaimID = "";
            TpaClaimID = "";
            ProgramID = "";
            ReasonCode = "";
            ReasonDesc = "";
            ResponseCode = "";
            ResponseDesc = "";
            SuccessCode = "";
            SuccessDesc = "";
            User = "";
            PassWord = "";
            TimeStamp = "";
            Token = "";
            InitializeTotLen();
        }

        public CommonData(string transNumber, string seClaimID, string tpaClaimID,
            string programID, string reasonCode, string reasonDesc,
            string responseCode, string responseDesc,
            string successCode, string successDesc, string user,
            string passWord, string timeStamp, string token)
        {
            TransNumber = transNumber;
            SeClaimID = seClaimID;
            TpaClaimID = tpaClaimID;
            ProgramID = programID;
            ReasonCode = reasonCode;
            ReasonDesc = reasonDesc;
            ResponseCode = responseCode;
            ResponseDesc = responseDesc;
            SuccessCode = successCode;
            SuccessDesc = successDesc;
            User = user;
            PassWord = passWord;
            TimeStamp = timeStamp;
            Token = token;
            InitializeTotLen();
        }

        // VPay Data
        public string TransNumber { get; set; }
        public string SeClaimID { get; set; }
        public string TpaClaimID { get; set; }
        public string ProgramID { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDesc { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
        public string SuccessCode { get; set; }
        public string SuccessDesc { get; set; }
        public string User { get; set; }
        public string PassWord { get; set; }
        public string TimeStamp { get; set; }
        public string Token { get; set; }

        // Working Members Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int TotLen { get; set; }
        public int StrIdx { get; set; }
        public int EndIdx { get; set; }

        // field lengths
        private int _trnNbrLen = 20;
        private int _clamIdLen = 20;
        private int _tpaClmLen = 20;
        private int _progrmLen = 20;
        private int _rsCodeLen = 4;
        private int _rsDescLen = 256;
        private int _rpCodeLen = 4;
        private int _rpDescLen = 256;
        private int _scCodeLen = 4;
        private int _scDescLen = 256;
        private int _userLen = 10;
        private int _passwdLen = 10;
        private int _timeStLen = 26;
        private int _tokenLen = 128;

        private void InitializeTotLen()
        {
            TotLen = 10 +
                     _trnNbrLen + _clamIdLen + _tpaClmLen + _progrmLen +
                     _rsCodeLen + _rsDescLen + _rpCodeLen + _rpDescLen +
                     _scCodeLen + _scDescLen + _userLen + _passwdLen +
                     _timeStLen + _tokenLen;
        }

        public override string ToString()
        {
            // TODO: Implement complex logic
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
                TransNumber, SeClaimID, TpaClaimID, ProgramID, ReasonCode, ReasonDesc, ResponseCode, ResponseDesc, SuccessCode,
                SuccessDesc, User, PassWord, TimeStamp, Token
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
