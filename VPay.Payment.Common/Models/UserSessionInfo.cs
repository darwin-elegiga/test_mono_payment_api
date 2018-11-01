namespace VPay.Payment.Common
{
    public class UserSessionInfo
    {
        public string Token { get; set; }

        public string UserName { get; set; }

        public char Source { get; set; } // Source Of Request P-webPage, S-webSvc 
    }
}
