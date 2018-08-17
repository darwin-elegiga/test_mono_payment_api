using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CardData
    {
        public CardData()
        {
            CardType = "";
            CardNumber = "";
            CardCvv2 = "";
            CardExpiration = "";
            LoadTransId = "";
            LoadAmount = "";
            LoadFee = "";
            PayeeName = "";
            CardholderName = "";
            CardholderAddress = "";
            CardPostalCode = "";
            UnloadCode = "";
            UnloadDesc = "";
            VcRef = "";
            DisplayCvv2 = "";
            MaskPan = "";
            InitializeTotLen();
        }

        public CardData(string cardType, string cardNumber, string cardCvv2,
            string cardExpiration, string loadTransId, string loadAmount,
            string loadFee, string payeeName, string cardholderName,
            string cardholderAddress, string cardPostalCode,
            string unloadCode, string unloadDesc, string vcRef,
            string displayCvv2, string maskPan)
        {
            CardType = cardType;
            CardNumber = cardNumber;
            CardCvv2 = cardCvv2;
            CardExpiration = cardExpiration;
            LoadTransId = loadTransId;
            LoadAmount = loadAmount;
            LoadFee = loadFee;
            PayeeName = payeeName;
            CardholderName = cardholderName;
            CardholderAddress = cardholderAddress;
            CardPostalCode = cardPostalCode;
            UnloadCode = unloadCode;
            UnloadDesc = unloadDesc;
            VcRef = vcRef;
            DisplayCvv2 = displayCvv2;
            MaskPan = maskPan;
            InitializeTotLen();
        }

        // VPay Data
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv2 { get; set; }
        public string CardExpiration { get; set; }
        public string LoadTransId { get; set; }
        public string LoadAmount { get; set; }
        public string LoadFee { get; set; }
        public string PayeeName { get; set; }
        public string CardholderName { get; set; }
        public string CardholderAddress { get; set; }
        public string CardPostalCode { get; set; }
        public string UnloadCode { get; set; }
        public string UnloadDesc { get; set; }
        public string VcRef { get; set; }
        public string DisplayCvv2 { get; set; }
        public string MaskPan { get; set; }

        //working Members
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        //Field Lengths
        private int _crdTypLen = 50;
        private int _crdNumLen = 16;
        private int _crCvv2Len = 4;
        private int _crdExpLen = 4;
        private int _lodTrnLen = 15;
        private int _lodAmtLen = 19;
        private int _lodFeeLen = 6;
        private int _payNamLen = 50;
        private int _crdNamLen = 50;
        private int _crdAdrLen = 30;
        private int _crdZipLen = 15;
        private int _unCodeLen = 10;
        private int _unDescLen = 60;
        private int _vcRefLen = 20;
        private int _dsCvv2Len = 1;
        private int _maskPLen = 1;


        private void InitializeTotLen()
        {
            TotLen = 10 +
                     _crdTypLen + _crdNumLen + _crCvv2Len + _crdExpLen + _lodTrnLen +
                     _lodAmtLen + _lodFeeLen + _payNamLen + _crdNamLen + _crdAdrLen +
                     _crdZipLen + _unCodeLen + _unDescLen + _vcRefLen + _dsCvv2Len +
                     _maskPLen;
        }

        public override string ToString()
        {
            // TODO:  Do complex logic
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
                CardType, CardNumber, CardCvv2, CardExpiration, LoadTransId, LoadAmount, LoadFee, PayeeName, CardholderName, CardholderAddress,
                CardPostalCode, UnloadCode, UnloadDesc, VcRef, DisplayCvv2, MaskPan
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
