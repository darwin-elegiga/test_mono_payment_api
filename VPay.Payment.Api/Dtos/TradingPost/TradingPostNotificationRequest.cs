using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostNotificationRequest
    {
        [JsonProperty("NotificationRequest")]
        public TradingPostData.ReleaseNotification NotificationRequest { get; set; }

        [JsonProperty("AuthenticationValues")]
        public TradingPostData.AuthenticationValuesAndIp AuthenticationValues { get; set; }
    }
}
