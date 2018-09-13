using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class SwitchTransaction
    {
        public SwitchTransaction()
        {
            AcquireID = "";
            AuthCode = "";
            AvailableBal = "";
            CaptureTS = "";
            CurrentBal = "";
            MerchantID = "";
            OlsLogID = "";
            Stan = "";
            Switch = "";
            TerminalID = "";
            TransactionTS = "";
            InitializeTotLen();
        }

        public SwitchTransaction(string acquireID, string authCode, string availableBal,
            string captureTS, string currentBal, string merchantID,
            string olsLogID, string stan, string switch1,
            string terminalID, string transactionTS)
        {
            AcquireID = acquireID;
            AuthCode = authCode;
            AvailableBal = availableBal;
            CaptureTS = captureTS;
            CurrentBal = currentBal;
            MerchantID = merchantID;
            OlsLogID = olsLogID;
            Stan = stan;
            Switch = switch1;
            TerminalID = terminalID;
            TransactionTS = transactionTS;
            InitializeTotLen();
        }

        // VPay Data
        public string AcquireID { get; set; }
        public string AuthCode { get; set; }
        public string AvailableBal { get; set; }
        public string CaptureTS { get; set; }
        public string CurrentBal { get; set; }
        public string MerchantID { get; set; }
        public string OlsLogID { get; set; }
        public string Stan { get; set; }
        public string Switch { get; set; }
        public string TerminalID { get; set; }
        public string TransactionTS { get; set; }

        // Working Members Not In Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        // member lengths
        private int _acqireLen = 20;
        private int _athCodLen = 30;
        private int _avlBalLen = 20;
        private int _captTSLen = 26;
        private int _curBalLen = 20;
        private int _mrchntLen = 20;
        private int _olsLogLen = 12;
        private int _stanLen = 20;
        private int _switchLen = 10;
        private int _trmnIdLen = 20;
        private int _trnsTSLen = 26;

        private void InitializeTotLen()
        {
            TotLen = 10 +
                _acqireLen + _athCodLen + _avlBalLen + _captTSLen +
                _curBalLen + _mrchntLen + _olsLogLen +
                _stanLen + _switchLen + _trmnIdLen + _trnsTSLen;
        }

        public override string ToString()
        {
            // TODO:  complex logic
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
                AcquireID, AuthCode, AvailableBal, CaptureTS, CurrentBal, MerchantID, OlsLogID, Stan, Switch, TerminalID, TransactionTS
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

            builder.Append("SWITCHTRAN".PadRight(10));
            builder.Append(AcquireID.PadRight(_acqireLen));
            builder.Append(AuthCode.PadLeft(_athCodLen));
            builder.Append(AvailableBal.PadLeft(_avlBalLen));
            builder.Append(CaptureTS.PadRight(_captTSLen));
            builder.Append(CurrentBal.PadLeft(_curBalLen));
            builder.Append(MerchantID.PadRight(_mrchntLen));
            builder.Append(OlsLogID.PadRight(_olsLogLen));
            builder.Append(Stan.PadRight(_stanLen));
            builder.Append(Switch.PadRight(_switchLen));
            builder.Append(TerminalID.PadRight(_trmnIdLen));
            builder.Append(TransactionTS.PadRight(_trnsTSLen));

            return builder.ToString();
        }

        public void HydrateFromISeriesString(ref string iSeriesString)
        {
            int index = 10;
            if (iSeriesString.Substring(0, 10) != "SWITCHTRAN") return; // TODO: Handle weird errors better

            AcquireID = ReadNext(ref iSeriesString, ref index, _acqireLen);
            AuthCode = ReadNext(ref iSeriesString, ref index, _athCodLen);
            AvailableBal = ReadNext(ref iSeriesString, ref index, _avlBalLen);
            CaptureTS = ReadNext(ref iSeriesString, ref index, _captTSLen);
            CurrentBal = ReadNext(ref iSeriesString, ref index, _curBalLen);
            MerchantID = ReadNext(ref iSeriesString, ref index, _mrchntLen);
            OlsLogID = ReadNext(ref iSeriesString, ref index, _olsLogLen);
            Stan = ReadNext(ref iSeriesString, ref index, _stanLen);
            Switch = ReadNext(ref iSeriesString, ref index, _switchLen);
            TerminalID = ReadNext(ref iSeriesString, ref index, _trmnIdLen);
            TransactionTS = ReadNext(ref iSeriesString, ref index, _trnsTSLen);
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
