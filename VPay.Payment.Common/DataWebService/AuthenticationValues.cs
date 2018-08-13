using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.DataWebService
{
    public class AuthenticationValues
    {
        public AuthenticationValues()
        {
            Id = "";
            PassPhrase = "";
        }

        public AuthenticationValues(string id, string passPhrase)
        {
            Id = id;
            PassPhrase = passPhrase;
        }

        public string Id { get; set; }
        public string PassPhrase { get; set; }

        public override string ToString()
        {
            return "AuthenticationValues [_id=" + Id + ", _passPhrase=" + PassPhrase + "]";
        }

        public Boolean IsValid()
        {
            bool returnValue = true;

            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(PassPhrase))
                returnValue = false;

            return returnValue;
        }
    }
}
