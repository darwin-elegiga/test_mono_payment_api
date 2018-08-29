using System.Threading.Tasks;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;
using VPay.Payment.Common.Models;

namespace VPay.Payment.Common
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated(AuthenticationValues av, string ipAddress);

        Task<AuthenticationResult> TestAuthentication(AuthenticationParam param);

        Task<UserSessionInfo> Login(AuthenticationParam param);

        Task<bool> IsAuthorized(string userId, string webServiceName, string action);

    }
}
