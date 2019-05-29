using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostLoadRequest
    {
        [JsonProperty("loadRequest")]
        public TradingPostData.LoadRequest LoadRequest { get; set; }

        [JsonProperty("authentication")]
        public AuthenticationValues AuthenticationValues { get; set; }
    }
}
