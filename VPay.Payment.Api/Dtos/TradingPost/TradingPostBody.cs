using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostBody
    {
        [JsonProperty("LoadCard")]
        public TradingPostLoadRequest LoadCard { get; set; }

        [JsonProperty("RetrieveCard")]
        public TradingPostRetrieveRequest RetrieveCard { get; set; }

        [JsonProperty("CardNotificationRelease")]
        public TradingPostNotificationRequest CardNotificationRelease { get; set; }
    }
}
