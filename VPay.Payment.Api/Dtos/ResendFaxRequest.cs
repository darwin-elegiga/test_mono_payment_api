using System;
using Newtonsoft.Json;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class ResendFaxRequest : FaxRequest
    {
        public string FaxNumber { get; set; }

        [JsonIgnore]
        public string CleanFaxNumber => FaxNumber.CleanFaxNumber();
    }
}
