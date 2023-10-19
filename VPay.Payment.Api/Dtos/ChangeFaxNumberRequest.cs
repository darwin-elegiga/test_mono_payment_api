using System;
using Newtonsoft.Json;
using VPay.Payment.Common;
namespace VPay.Payment.Api.Dtos
{
    public class ChangeFaxNumberRequest : FaxRequest
    {
        [Obsolete("Comment: This API endpoint is no longer used and has been deprecated")]
        public string FaxNumber { get; set; }

        [JsonIgnore]
        [Obsolete("Comment: This API endpoint is no longer used and has been deprecated")]
        public string CleanFaxNumber => FaxNumber.CleanFaxNumber();
    }
}
