namespace VPay.Payment.Common.Models
{
    public class PaymentConfig
    {
        public bool ValidateIP { get; set; } = true;

        public bool UseCheckEmail { get; set; } = false;
    }
}
