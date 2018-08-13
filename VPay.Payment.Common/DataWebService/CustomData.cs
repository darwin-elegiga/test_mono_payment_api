using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class CustomData
    {
        public CustomData()
        {
            ClientData = "";
        }

        public string ClientData { get; set; }
        public string ClientDataFixed { get; set; }
        public static int ClientDataLen
        {
            get { return 1024; }
        }

    }
}
