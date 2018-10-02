
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class ChangeFaxNumberRequest : FaxRequest
    {
        public string FaxNumber { get; set; }

        [JsonIgnore]
        public string CleanFaxNumber => FaxNumber.CleanFaxNumber();
    }
}
