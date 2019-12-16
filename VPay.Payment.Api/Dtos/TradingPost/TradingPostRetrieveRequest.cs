using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostRetrieveRequest
    {
        [JsonProperty("retrieveRequest")]
        public TradingPostData.RetrieveRequest RetrieveRequest { get; set; }

        [JsonProperty("authentication")]
        public AuthenticationValues AuthenticationValues { get; set; }
    }
}
