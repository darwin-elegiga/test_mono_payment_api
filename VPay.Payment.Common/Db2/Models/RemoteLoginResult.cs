namespace VPay.Payment.Common.Db2
{
    public class RemoteLoginResult
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ReturnCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Guid { get; set; }
        public string Token { get; set; }
        public string Source { get; set; }

    }
}
