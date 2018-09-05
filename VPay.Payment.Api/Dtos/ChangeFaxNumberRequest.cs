
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class ChangeFaxNumberRequest
    {
        public int? FaxCode { get; set; }

        public string FaxNumber { get; set; }

        [JsonIgnore]
        public string CleanFaxNumber
        {
            get
            {
                if (FaxNumber == null)
                {
                    return null;
                }

                return Regex.Replace(FaxNumber, "^[1]|[.|(|)| |_|-]", "");
            }
        }
    }
}
