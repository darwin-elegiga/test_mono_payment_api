using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostRequest
    {
        [JsonProperty("Envelope")]
        public TradingPostEnvelope Envelope { get; set; }
    }
}
