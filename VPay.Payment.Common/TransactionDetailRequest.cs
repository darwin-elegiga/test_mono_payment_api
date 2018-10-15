using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class TransactionDetailRequest
    {
        public TransactionDetailRequest()
        {
            User = "";
            TransNumber = "";
        }

        public string User { get; set; }
        public string Token { get; set; }

        public string TransNumber { get; set; }


    }
}
