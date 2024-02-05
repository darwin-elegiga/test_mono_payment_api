using System;
using System.Text;
using System.Text.RegularExpressions;

namespace VPay.Payment.Common
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
            if (String.IsNullOrEmpty(faxNumber))
            {
                return null;
            }

            return Regex.Replace(faxNumber, "^[1]|[.|(|)| |_|-]", "");
        }

        //based off https://stackoverflow.com/a/51214178, but changed to string builder
        public static string CleanWordValues(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var builder = new StringBuilder();

            foreach (var c in input)
            {
                switch (c)
                {
                    case '\u2013':
                        // en dash
                        builder.Append('-');
                        break;

                    case '\u2014':
                        // em dash
                        builder.Append('-');
                        break;

                    case '\u2015':
                        // horizontal bar
                        builder.Append('-');
                        break;

                    case '\u2017':
                        // double low line
                        builder.Append('_');
                        break;

                    case '\u2018':
                        // left single quotation mark
                        builder.Append('\'');
                        break;

                    case '\u2019':
                        // right single quotation mark
                        builder.Append('\'');
                        break;

                    case '\u201a':
                        // single low-9 quotation mark
                        builder.Append(',');
                        break;

                    case '\u201b':
                        // single high-reversed-9 quotation mark
                        builder.Append('\'');
                        break;

                    case '\u201c':
                        // left double quotation mark
                        builder.Append('\"');
                        break;

                    case '\u201d':
                        // right double quotation mark
                        builder.Append('\"');
                        break;

                    case '\u201e':
                        // double low-9 quotation mark
                        builder.Append('\"');
                        break;

                    case '\u2026':
                        // horizontal ellipsis
                        builder.Append("...");
                        break;

                    case '\u2032':
                        // prime
                        builder.Append('\'');
                        break;

                    case '\u2033':
                        // double prime
                        builder.Append('\"');
                        break;

                    default:
                        builder.Append(c);
                        break;
                }
            }
            return builder.ToString();
        }
    }
}
