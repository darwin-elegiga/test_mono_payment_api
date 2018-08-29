using System;
using System.Collections.Generic;
using System.Text;
using VPay.Payment.Common;

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

        public string Token
        {
            get;
            set;
        }

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

        public string ToISeriesString()
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("COMMONDATA".PadRight(10));
            builder.Append(TransNumber.PadLeft(_trnNbrLen));
            builder.Append(SeClaimID.PadRight(_clamIdLen));
            builder.Append(TpaClaimID.PadRight(_tpaClmLen));
            builder.Append(ProgramID.PadRight(_progrmLen));
            builder.Append(ReasonCode.PadRight(_rsCodeLen));
            builder.Append(ReasonDesc.PadRight(_rsDescLen));
            builder.Append(ResponseCode.PadRight(_rpCodeLen));
            builder.Append(ResponseDesc.PadRight(_rpDescLen));
            builder.Append(SuccessCode.PadRight(_scCodeLen));
            builder.Append(SuccessDesc.PadRight(_scDescLen));
            builder.Append(User.PadRight(_userLen));
            builder.Append(PassWord.PadRight(_passwdLen));
            builder.Append(TimeStamp.PadRight(_timeStLen));
            builder.Append(Token.PadRight(_tokenLen));

            return builder.ToString();
        }

        public void HydrateFromISeriesString(ref string iSeriesString)
        {
            int index = 10;
            if (iSeriesString.Substring(0, 10) != "COMMONDATA") return; // TODO: Handle weird errors better

            TransNumber = ReadNext(ref iSeriesString, ref index, _trnNbrLen);
            SeClaimID = ReadNext(ref iSeriesString, ref index, _clamIdLen);
            TpaClaimID = ReadNext(ref iSeriesString, ref index, _tpaClmLen);
            ProgramID = ReadNext(ref iSeriesString, ref index, _progrmLen);
            ReasonCode = ReadNext(ref iSeriesString, ref index, _rsCodeLen);
            ReasonDesc = ReadNext(ref iSeriesString, ref index, _rsDescLen);
            ResponseCode = ReadNext(ref iSeriesString, ref index, _rpCodeLen);
            ResponseDesc = ReadNext(ref iSeriesString, ref index, _rpDescLen);
            SuccessCode = ReadNext(ref iSeriesString, ref index, _scCodeLen);
            SuccessDesc = ReadNext(ref iSeriesString, ref index, _scDescLen);
            User = ReadNext(ref iSeriesString, ref index, _userLen);
            PassWord = ReadNext(ref iSeriesString, ref index, _passwdLen);
            TimeStamp = ReadNext(ref iSeriesString, ref index, _timeStLen);
            Token = ReadNext(ref iSeriesString, ref index, _tokenLen);
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
