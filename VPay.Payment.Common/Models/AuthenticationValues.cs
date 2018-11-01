namespace VPay.Payment.Common
{
    public class AuthenticationValues
    {

        public string Id { get; set; } = "";
        public string PassPhrase { get; set; } = "";

        public override string ToString()
        {
            return "AuthenticationValues [_id=" + Id + ", _passPhrase=" + PassPhrase + "]";
        }

    }
}
