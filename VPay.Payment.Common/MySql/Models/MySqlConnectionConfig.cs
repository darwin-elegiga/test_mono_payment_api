namespace VPay.Payment.Common.MySql
{
    public class MySqlConnectionConfig
    {

        public string Hostname { get; set; }
        public uint Port { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SslMode { get; set; }
        public bool CheckParameters { get; set; }

    }
}
