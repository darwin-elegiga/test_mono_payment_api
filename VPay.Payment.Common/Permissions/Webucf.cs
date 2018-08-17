using System;

namespace VPay.Payment.Common.Permissions
{
    public class Webucf // done
    {
        public Webucf()
        {

        }

        public Webucf(string wuuser, string wupass, string wuusrc, string wusessid, DateTime? wulast)
        {
            Wuuser = wuuser;
            Wupass = wupass;
            Wuusrc = wuusrc;
            Wusessid = wusessid;
            Wulast = wulast;
        }

        public int Keyid { get; set; }
        public string Wuuser { get; set; }
        public string Wupass { get; set; }
        public string Wuusrc { get; set; }
        public string Wusessid { get; set; }
        public DateTime? Wulast { get; set; }
    }
}
