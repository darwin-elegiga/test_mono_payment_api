using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostNotificationRequest
    {
        [JsonProperty("releaseNotification")]
        public TradingPostData.ReleaseNotification NotificationRequest { get; set; }

        [JsonProperty("authentication")]
        public AuthenticationValues AuthenticationValues { get; set; }
    }
}
