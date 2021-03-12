namespace VPay.Payment.Common
{
    public class TransactionDetailRequest
    {
        public string User { get; set; } = "";
        public string Token { get; set; } = "";
        public string TransNumber { get; set; } = "";
        public char Source { get; set; } = 'P';
    }
}
