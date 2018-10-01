using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyRequest
    {

        [JsonProperty("Envelope")]
        public LegacyEnvelope Envelope { get; set; }
    }
}
