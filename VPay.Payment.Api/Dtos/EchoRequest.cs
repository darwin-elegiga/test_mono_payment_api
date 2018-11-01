using VPay.Payment.Common;

namespace VPay.Payment.Api.Dtos
{
    public class EchoRequest
    {
        public AuthenticationValues Av { get; set; }

        public string Es { get; set; }
    }
}
