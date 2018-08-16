using System;

namespace VPay.Payment.Common.MySql
{
    public class SessionEntry
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string SessionId { get; set; }
        public DateTime DateHit { get; set; }
        public bool Active { get; set; }
        public string Data { get; set; } = " ";
    }
}
