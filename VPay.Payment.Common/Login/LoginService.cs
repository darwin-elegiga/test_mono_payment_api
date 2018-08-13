using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.Login
{
    public class LoginService
    {
        public LoginService(string name, string password, string uniqueid, string recordid)
        {

        }

        public string Username { get; set; }
        public string Uniqueid { get; set; }
        public string Password { get; set; }
        public string Recordid { get; set; }
        public string LogError { get; set; }
        public string Advice { get; set; }

    }
}
