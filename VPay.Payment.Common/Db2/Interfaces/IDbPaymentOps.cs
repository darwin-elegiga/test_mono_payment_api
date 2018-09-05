using System;
using System.Threading.Tasks;

namespace VPay.Payment.Common.Db2
{
    public interface IDbPaymentOps : IDisposable
    {
        Task<AuthenticationResult> AuthenticateUser(AuthenticationParam param);

        Task<RemoteLoginResult> RemoteLogin(string username, string password, string source);

        Task<SecurityCheckResult> CheckUserSecurity(SecurityCheckParam param);

        Task<string> BalanceRequest(string auth, string password, string ip, string data);


        
        Task<FaxMethodResult> CancelFax(string token, int faxCode);
        
        Task<FaxMethodResult> ChangeFaxNumber(string token, int faxCode, string phoneNumber);

        Task<FaxMethodResult> HoldFax(string token, int faxCode);

        Task<FaxMethodResult> ReleaseFax(string token, int faxCode);

        Task<FaxMethodResult> ResendFax(string token, int faxCode, string phoneNumber);
    }
}
