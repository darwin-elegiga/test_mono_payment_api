using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyEnvelopeDto
    {

        [JsonProperty("Envelope")]
        public Envelope Envelope { get; set; }
    }


    public class Envelope
    {
        [JsonProperty("Body")]
        public Body Body { get; set; }
    }


    public class Body
    {

        [JsonProperty("GetReasonCodes")]
        public ReasonCodeExpectedRequest GetReasonCodes { get; set; }

        [JsonProperty("GetTransactionDetails")]
        public TransactionDetailExpectedRequest GetTransactionDetails { get; set; }

        [JsonProperty("GetPanNumber")]
        public StandardExpectedRequest GetPanNumber { get; set; }

        [JsonProperty("OpenPreAuth")]
        public StandardExpectedRequest OpenPreAuth { get; set; }

        [JsonProperty("BalanceRequest")]
        public StandardExpectedRequest BalanceRequest { get; set; }

        [JsonProperty("UnloadPan")]
        public StandardExpectedRequest UnloadPan { get; set; }

        [JsonProperty("StopPay")]
        public StandardExpectedRequest StopPay { get; set; }

        [JsonProperty("LoadPan")]
        public LoadPanExpectedRequest LoadPan { get; set; }

        [JsonProperty("CancelFax")]
        public StandardExpectedRequest CancelFax { get; set; }

        [JsonProperty("ChangeFaxNumber")]
        public StandardExpectedRequest ChangeFaxNumber { get; set; }

        [JsonProperty("HoldFax")]
        public StandardExpectedRequest HoldFax { get; set; }

        [JsonProperty("ReleaseFax")]
        public StandardExpectedRequest ReleaseFax { get; set; }

        [JsonProperty("ResendFax")]
        public StandardExpectedRequest ResendFax { get; set; }
        
    }

    public class StandardExpectedRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("sr")]
        public StandardRequest Request { get; set; }
    }

    public class LoadPanExpectedRequest : StandardExpectedRequest
    {
        [JsonProperty("cd")]
        public CustomData CustomData { get; set; }
    }

    public class CustomData
    {
        public string ClientData { get; set; }
    }
    
    public class ReasonCodeExpectedRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("reasonCodeRequest")]
        public ReasonCodeRequest Request { get; set; }
    }

    public class TransactionDetailExpectedRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("transactionDetailRequest")]
        public TransactionDetailRequest Request { get; set; }
    }

}
