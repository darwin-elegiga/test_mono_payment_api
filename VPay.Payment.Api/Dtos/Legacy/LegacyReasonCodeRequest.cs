using Newtonsoft.Json;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyReasonCodeRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("reasonCodeRequest")]
        public ReasonCodeRequestDto Request { get; set; }
    }

}
