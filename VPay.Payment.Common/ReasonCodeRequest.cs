using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class ReasonCodeRequest
    {
        public ReasonCodeRequest()
        {
            User = "";
            PassWord = "";
            TransNumber = "";
            Source = ' '; // Source of request P-webPage, S-webSvc, ' '-Other
        }

        public ReasonCodeRequest(string user, string passWord,
            string txid, char source)
        {
            User = user;
            PassWord = passWord;
            TransNumber = txid;
            Source = source;
        }

        public string User { get; set; }
        public string PassWord { get; set; }
        public string TransNumber { get; set; }

        public char Source { get; set; }

        public override string ToString()
        {
            // TODO:  Do real logic
            return "";
        }

        public bool IsValid()
        {
            // Strange, but it's existing logic
            return true;
        }
    }
}
