using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    [Serializable]
    public class ReasonCode
    {
        public string ReasonCodeString { get; set; }
        public string ReasonDesc { get; set; }
        public string ReasonAdsc { get; set; }
        /* M001
        public string ReasonAtyp { get; set; }
        public string ReasonAflg { get; set; }
        *
        */

    }
}
