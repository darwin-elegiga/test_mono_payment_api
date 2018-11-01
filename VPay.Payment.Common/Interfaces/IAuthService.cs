using System.Threading.Tasks;

namespace VPay.Payment.Common
{
    public interface IAuthService
    {
        Task<AuthenticationResult> TestAuthentication(AuthenticationParam param);

        Task<UserSessionInfo> Login(AuthenticationParam param);

        Task<bool> IsAuthorized(string userId, string webServiceName, string action);

    }
}
