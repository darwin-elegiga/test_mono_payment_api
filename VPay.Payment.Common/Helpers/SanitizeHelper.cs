using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VPay.Payment.Common.Helpers
{
    public static class SanitizeHelper
    {
        public static string MaskUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "[redacted]";
            }

            if (userName.Length <= 2)
            {
                return "***";
            }

            return userName.Substring(0, 2) + "***";
        }

        public static string GetDeterministicHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        public static string MaskForLogging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length <= 2)
            {
                return "**";
            }

            return $"{value.Substring(0, 1)}***{value.Substring(value.Length - 1, 1)}";
        }


    }
}
   

