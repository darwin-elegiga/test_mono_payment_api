using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostEnvelope
    {
        [JsonProperty("Body")]
        public TradingPostBody Body { get; set; }
    }
}
