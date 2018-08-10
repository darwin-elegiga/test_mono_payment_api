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
            PassWord = "";
            Txid = "";
            Source = ' ';
        }

        public TransactionDetailRequest(string user, string passWord,
            string txid, char source)
        {
            User = user;
            PassWord = passWord;
            Txid = txid;
            Source = source;
        }

        public string User { get; set; }
        public string PassWord { get; set; }
        public string Txid { get; set; }
        public char Source { get; set; } // Source of request P-webPage, S-webSvc, ' '-Other

        public override string ToString()
        {
            // TODO:  Implement real logic

            return "";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
