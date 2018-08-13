using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CoveredItem
    {
        public CoveredItem()
        {
            ItemType = "";
            ItemId = "";
            ItemYear = "";
            Manufacturer = "";
            Model = "";
            BookStateOrProvince = "";
            PostalCode = "";
            PlanCode = "";
            PlanDescription = "";
            Deductible = "";
            NewUsed = "";
            BeginDate = "";
            ExpireDate = "";
            OdometerType = "";
            BeginOdometer = "";
            ExpireOdometer = "";
            OwnerLastName = "";
            OwnerFirstName = "";
            InitializeTotLen();
        }

        public CoveredItem(string itemType, string itemId, string itemYear, string manufacturer, string model,
            string bookStateOrProvince, string postalCode, string planCode, string planDescription, string deductible,
            string newUsed, string beginDate, string expireDate, string odometerType, string beginOdometer,
            string expireOdometer, string ownerLastName, string ownerFirstName)
        {
            ItemType = itemType;
            ItemId = itemId;
            ItemYear = itemYear;
            Manufacturer = manufacturer;
            Model = model;
            BookStateOrProvince = bookStateOrProvince;
            PostalCode = postalCode;
            PlanCode = planCode;
            PlanDescription = planDescription;
            Deductible = deductible;
            NewUsed = newUsed;
            BeginDate = beginDate;
            ExpireDate = expireDate;
            OdometerType = odometerType;
            BeginOdometer = beginOdometer;
            ExpireOdometer = expireOdometer;
            OwnerLastName = ownerLastName;
            OwnerFirstName = ownerFirstName;
            InitializeTotLen();
        }


        public string ItemType { get; set; }
        public string ItemId { get; set; }
        public string ItemYear { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string BookStateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string PlanCode { get; set; }
        public string PlanDescription { get; set; }
        public string Deductible { get; set; }
        public string NewUsed { get; set; }
        public string BeginDate { get; set; }
        public string ExpireDate { get; set; }
        public string OdometerType { get; set; }
        public string BeginOdometer { get; set; }
        public string ExpireOdometer { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerFirstName { get; set; }

        // Working Members Not Part Of Wsdl
        public string ParseResult { get; set; }
        public string LengthError { get; set; }

        public int StrIdx { get; set; }
        public int EndIdx { get; set; }
        public int TotLen { get; set; }

        // member lengths
        private int _itmTypLen = 20;
        private int _itemIdLen = 20;
        private int _itemYrLen = 4;
        private int _itmMfcLen = 20;
        private int _modelLen = 20;
        private int _itmStpLen = 3;
        private int _itmZipLen = 15;
        private int _plnCodLen = 50;
        private int _plnDscLen = 50;
        private int _deductLen = 8;
        private int _newUsdLen = 1;
        private int _begDatLen = 8;
        private int _expDatLen = 8;
        private int _odoTypLen = 4;
        private int _odoBegLen = 7;
        private int _odoExpLen = 7;
        private int _ownLnmLen = 35;
        private int _ownFnmLen = 35;

        private void InitializeTotLen()
        {
            TotLen = 10 +
                _itmTypLen + _itemIdLen + _itemYrLen + _itmMfcLen + _modelLen + _itmStpLen +
                _itmZipLen + _plnCodLen + _plnDscLen + _deductLen + _newUsdLen + _begDatLen +
                _expDatLen + _odoTypLen + _odoBegLen + _odoExpLen + _ownLnmLen + _ownFnmLen;
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
