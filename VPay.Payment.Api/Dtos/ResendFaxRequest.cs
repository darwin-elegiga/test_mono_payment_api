using System;
using VPay.Payment.Common;
using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class ResendFaxRequest : FaxRequest  //ChangeFaxNumberRequest
    {
        //DeveloperNote: ChangeFaxNumberRequest is a parentClass for ResendFaxRequest so needs to copy below property has to move on Resendfaxrequest class

        public string FaxNumber { get; set; }

        //[JsonIgnore]
        //public string CleanFaxNumber => FaxNumber.CleanFaxNumber();
    }
}
