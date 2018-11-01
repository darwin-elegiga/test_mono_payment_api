using Newtonsoft.Json;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class LegacyTransactionDetailRequest
    {
        [JsonProperty("av")]
        public AuthenticationValues AuthenticationValues { get; set; }

        [JsonProperty("transactionDetailRequest")]
        public TransactionDetailRequestDto Request { get; set; }
    }

}
