namespace VPay.Payment.Common
{
    public interface IUserInfo
    {
        string UserName { get; }

        string Token { get; }

        string Source { get; }
    }
}
