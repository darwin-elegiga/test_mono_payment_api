using Newtonsoft.Json;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyReasonCodeRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("reasonCodeRequest")]
        public ReasonCodeRequest Request { get; set; }
    }

}
