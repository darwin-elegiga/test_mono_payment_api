using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Dtos
{
    public class TradingPostRetrieveRequest
    {
        [JsonProperty("RetrieveRequest")]
        public TradingPostData.RetrieveRequest RetrieveRequest { get; set; }
    }
}
