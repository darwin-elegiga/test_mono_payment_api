namespace VPay.Payment.Common
{
    public class Detail
    {
        public long TranId { get; set; }
        public long LoadTran { get; set; }
        public string Status { get; set; }
        public string Amount { get; set; }
        public string AuthCode { get; set; }
        public string TranTimeStamp { get; set; }
        public string Expiration { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDesc { get; set; }
        public string ActionCode { get; set; }
        public string ActionDesc { get; set; }
        public string FinancialType { get; set; }
        public string RequesterName { get; set; }
        public string BatchNumber { get; set; }
        public string StatusDesc { get; set; }
    }
}
