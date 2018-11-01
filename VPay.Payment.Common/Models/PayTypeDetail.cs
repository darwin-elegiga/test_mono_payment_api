namespace VPay.Payment.Common
{
    public class PayTypeDetail
    {
        public PayTypeDetail()
        {
            Association = "";
            Bank = "";
            CardCvv2 = "";
            CardExp = "";
            CardNumber = "";
            OutsideCheck = "";
            ClearCheck = "";
            PosPayCheck = "";
            SwitchNumber = "";
        }

        public string Association { get; set; }
        public string Bank { get; set; }
        public string CardCvv2 { get; set; }
        public string CardExp { get; set; }
        public string CardNumber { get; set; }
        public string OutsideCheck { get; set; }
        public string ClearCheck { get; set; }
        public string PosPayCheck { get; set; }
        public string SwitchNumber { get; set; }


    }
}
