namespace VPay.Payment.Common.Db2
{
    public class SecurityCheckParam
    {
        public string UserId { get; set; }

        public string WebServiceName { get; set; }

        public string SecurityGroup { get; set; } = "WS_PUBLIC";

        public string Action { get; set; }
    }
}
