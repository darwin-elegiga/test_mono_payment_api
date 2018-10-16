using Newtonsoft.Json;

namespace VPay.Payment.Api.Dtos
{
    public class ReasonCodeRequestDto
    {

        [JsonProperty("user")]
        public string User { get; set; } = "";
        [JsonProperty("passWord")]
        public string PassWord { get; set; } = "";
        [JsonProperty("transNumber")]
        public string TransNumber { get; set; } = "";

    }
}
