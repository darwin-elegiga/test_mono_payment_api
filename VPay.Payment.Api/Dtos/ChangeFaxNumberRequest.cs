using System;
using Newtonsoft.Json;
using VPay.Payment.Common;
namespace VPay.Payment.Api.Dtos
{
    public class ChangeFaxNumberRequest : FaxRequest
    {
        //DeveloperNote: ChangeFaxNumberRequest is a parentClass for ResendFaxRequest so needs to copy below property has to move on Resendfaxrequest class
        [Obsolete("Comment: This API endpoint is no longer used and has been deprecated")]
        public string FaxNumber { get; set; }

        [JsonIgnore]
        [Obsolete("Comment: This API endpoint is no longer used and has been deprecated")]
        public string CleanFaxNumber => FaxNumber.CleanFaxNumber();
    }
}
