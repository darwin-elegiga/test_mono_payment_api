namespace VPay.Payment.Common
{
    public class PayTypeDetail
    {
        public PayTypeDetail()
        {
            Association = "";
            Bank = "";
            CardCvv2 = ""; /*m001*/
            CardExp = "";
            CardNumber = "";
            OutsideCheck = "";
            ClearCheck = "";
            PosPayCheck = "";
            SwitchNumber = "";
        }

        public PayTypeDetail(string association, string cardCvv2, string cardExp, /*m001*/
            string cardNumber, string outsideCheck, string clearCheck,
            string posPayCheck, string switchNumber, string bank)
        {
            Association = association;
            Bank = bank;
            CardCvv2 = cardCvv2; /*m001*/
            CardExp = cardExp;
            CardNumber = cardNumber;
            OutsideCheck = outsideCheck;
            ClearCheck = clearCheck;
            PosPayCheck = posPayCheck;
            SwitchNumber = switchNumber;
        }

        public string Association { get; set; }
        public string Bank { get; set; }
        public string CardCvv2 { get; set; } /*m001*/
        public string CardExp { get; set; }
        public string CardNumber { get; set; }
        public string OutsideCheck { get; set; }
        public string ClearCheck { get; set; }
        public string PosPayCheck { get; set; }
        public string SwitchNumber { get; set; }


    }
}
