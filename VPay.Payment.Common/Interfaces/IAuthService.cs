using System.Threading.Tasks;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Common
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated(AuthenticationValues av, string ipAddress);

        Task<AuthenticationResult> TestAuthentication(AuthenticationParam param);

        Task<bool> Login(Common.DataWebService.CommonData cd);
    }
}
