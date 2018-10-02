using System.Text.RegularExpressions;

namespace VPay.Payment.Api
{
    public static class StringHelpers
    {
        public static string FirstCharacterToLower(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        public static string CleanFaxNumber(this string faxNumber)
        {
            if (faxNumber == null)
            {
                return null;
            }

            return Regex.Replace(faxNumber, "^[1]|[.|(|)| |_|-]", "");
        }
    }
}
