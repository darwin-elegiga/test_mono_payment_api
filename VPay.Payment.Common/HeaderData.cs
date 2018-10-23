namespace VPay.Payment.Common
{
    public class HeaderData
    {
        public int TransNumber { get; set; }
        public string Client { get; set; }
        public string BillCode { get; set; }
        public string BillType { get; set; }
        public string AvailBalance { get; set; }
        public string CurrentBalance { get; set; }
        public string SwitchAvailBal { get; set; }
        public string SwitchCurrentBal { get; set; }
        public string PayeeCode { get; set; }
        public string PayeeName { get; set; }
        public string ProviderName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string TaxId { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
    }
}
