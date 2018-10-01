using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyEnvelope
    {
        [JsonProperty("Body")]
        public LegacyBody Body { get; set; }
    }

}
