using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostBody
    {
        [JsonProperty("LoadCard")]
        public TradingPostLoadRequest LoadRequest { get; set; }
        [JsonProperty("RetrieveCard")]
        public TradingPostRetrieveRequest RetrieveRequest { get; set; }
        [JsonProperty("ReleaseNotification")]
        public TradingPostNotificationRequest NotificationRequest { get; set; }
    }
}
