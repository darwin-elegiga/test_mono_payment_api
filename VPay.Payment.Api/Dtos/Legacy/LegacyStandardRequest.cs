using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyStandardRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("sr")]
        public StandardRequest Request { get; set; }
    }
}
