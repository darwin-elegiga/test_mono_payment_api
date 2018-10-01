using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyLoadPanRequest : LegacyStandardRequest
    {
        [JsonProperty("cd")]
        public LegacyCustomData CustomData { get; set; }
    }

}
