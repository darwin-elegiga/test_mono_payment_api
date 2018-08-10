using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    [Serializable]
    public class HeaderRec
    {
        public HeaderRec()
        {

        }

        public HeaderRec(int providerId, string taxId, string providerName, string clientCode, int tranId,
            int pidTranslation,
            DateTime tranDate, string configId, string userKey, string userDsp1,
            string userDsp2, string userDsp3, string rqstId, string rqstName,
            double loadAmt, int expireYM, string cardType, string status,
            string clReason, string payeeCode, string payeeName, string custId,
            string billType, string billCode, string billCName, double curAmount,
            double avalAmount, DateTime loadTS, string acctCode, string addr1,
            string zipCode, int expireDt, string itemId, int itemYr,
            string itemMake, string itemModel, string itemState,
            string itemPlan, string itemPlanDesc, double itemDed,
            string itemNewUsed, int itemOrigMiles, string itemComplaint,
            int itemCurUse, string itemUseType, int claimDate,
            string clRepairOrder, string ownerFirst, string ownerLast,
            string merchZip, double claimDed, string clCrncy, string ldCrncy,
            string itemZip, int itemSvcDate, int itemExpDate,
            int itemExpOdom, string itemType, string description)
        {
            ProviderId = providerId;
            TaxId = taxId;
            ProviderName = providerName;
            ClientCode = clientCode;
            TranId = tranId;
            PidTranslation = pidTranslation;
            TranDate = tranDate;
            ConfigId = configId;
            UserKey = userKey;
            UserDsp1 = userDsp1;
            UserDsp2 = userDsp2;
            UserDsp3 = userDsp3;
            RqstId = rqstId;
            RqstName = rqstName;
            LoadAmt = loadAmt;
            ExpireYM = expireYM;
            CardType = cardType;
            Status = status;
            ClReason = clReason;
            PayeeCode = payeeCode;
            PayeeName = payeeName;
            CustId = custId;
            BillType = billType;
            BillCode = billCode;
            BillCName = billCName;
            CurAmount = curAmount;
            AvalAmount = avalAmount;
            LoadTS = loadTS;
            AcctCode = acctCode;
            Addr1 = addr1;
            ZipCode = zipCode;
            ExpireDt = expireDt;
            ItemId = itemId;
            ItemYr = itemYr;
            ItemMake = itemMake;
            ItemModel = itemModel;
            ItemState = itemState;
            ItemPlan = itemPlan;
            ItemPlanDesc = itemPlanDesc;
            ItemDed = itemDed;
            ItemNewUsed = itemNewUsed;
            ItemOrigMiles = itemOrigMiles;
            ItemComplaint = itemComplaint;
            ItemCurUse = itemCurUse;
            ItemUseType = itemUseType;
            ClaimDate = claimDate;
            ClRepairOrder = clRepairOrder;
            OwnerFirst = ownerFirst;
            OwnerLast = ownerLast;
            MerchZip = merchZip;
            ClaimDed = claimDed;
            ClCrncy = clCrncy;
            LdCrncy = ldCrncy;
            ItemZip = itemZip;
            ItemSvcDate = itemSvcDate;
            ItemExpDate = itemExpDate;
            ItemExpOdom = itemExpOdom;
            ItemType = itemType;
            Description = description;

        }

        public int ProviderId { get; set; }
        public string TaxId { get; set; }
        public string ProviderName { get; set; }
        public string ClientCode { get; set; }
        public int TranId { get; set; }
        public int PidTranslation { get; set; }
        public DateTime TranDate { get; set; }
        public string ConfigId { get; set; }
        public string UserKey { get; set; }
        public string UserDsp1 { get; set; }
        public string UserDsp2 { get; set; }
        public string UserDsp3 { get; set; }
        public string RqstId { get; set; }
        public string RqstName { get; set; }
        public double LoadAmt { get; set; }
        public int ExpireYM { get; set; }
        public string CardType { get; set; }
        public string Status { get; set; }
        public string ClReason { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeName { get; set; }
        public string CustId { get; set; }
        public string BillType { get; set; }
        public string BillCode { get; set; }
        public string BillCName { get; set; }
        public double CurAmount { get; set; }
        public double AvalAmount { get; set; }
        public DateTime LoadTS { get; set; }
        public string AcctCode { get; set; }
        public string Addr1 { get; set; }
        public string ZipCode { get; set; }
        public int ExpireDt { get; set; }
        public string ItemId { get; set; }
        public int ItemYr { get; set; }
        public string ItemMake { get; set; }
        public string ItemModel { get; set; }
        public string ItemState { get; set; }
        public string ItemPlan { get; set; }
        public string ItemPlanDesc { get; set; }
        public double ItemDed { get; set; }
        public string ItemNewUsed { get; set; }
        public int ItemOrigMiles { get; set; }
        public string ItemComplaint { get; set; }
        public int ItemCurUse { get; set; }
        public string ItemUseType { get; set; } // M - Miles, H - Hours
        public int ClaimDate { get; set; }
        public string ClRepairOrder { get; set; }
        public string OwnerFirst { get; set; }
        public string OwnerLast { get; set; }
        public string MerchZip { get; set; }
        public double ClaimDed { get; set; }
        public string ClCrncy { get; set; }
        public string LdCrncy { get; set; }
        public string ItemZip { get; set; }
        public int ItemSvcDate { get; set; }
        public int ItemExpDate { get; set; }
        public int ItemExpOdom { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }


    }
}
