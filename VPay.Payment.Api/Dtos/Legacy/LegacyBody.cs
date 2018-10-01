using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyBody
    {

        [JsonProperty("GetReasonCodes")]
        public LegacyReasonCodeRequest GetReasonCodes { get; set; }

        [JsonProperty("GetTransactionDetails")]
        public LegacyTransactionDetailRequest GetTransactionDetails { get; set; }

        [JsonProperty("GetPanNumber")]
        public LegacyStandardRequest GetPanNumber { get; set; }

        [JsonProperty("OpenPreAuth")]
        public LegacyStandardRequest OpenPreAuth { get; set; }

        [JsonProperty("BalanceRequest")]
        public LegacyStandardRequest BalanceRequest { get; set; }

        [JsonProperty("UnloadPan")]
        public LegacyStandardRequest UnloadPan { get; set; }

        [JsonProperty("StopPay")]
        public LegacyStandardRequest StopPay { get; set; }

        [JsonProperty("LoadPan")]
        public LegacyLoadPanRequest LoadPan { get; set; }

        [JsonProperty("CancelFax")]
        public LegacyStandardRequest CancelFax { get; set; }

        [JsonProperty("ChangeFaxNumber")]
        public LegacyStandardRequest ChangeFaxNumber { get; set; }

        [JsonProperty("HoldFax")]
        public LegacyStandardRequest HoldFax { get; set; }

        [JsonProperty("ReleaseFax")]
        public LegacyStandardRequest ReleaseFax { get; set; }

        [JsonProperty("ResendFax")]
        public LegacyStandardRequest ResendFax { get; set; }

    }

}
