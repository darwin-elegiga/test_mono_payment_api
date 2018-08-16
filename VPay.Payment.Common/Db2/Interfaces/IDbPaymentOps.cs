using System;
using System.Threading.Tasks;

namespace VPay.Payment.Common.Db2
{
    public interface IDbPaymentOps : IDisposable
    {
        Task<AuthenticationResult> AuthenticateUser(AuthenticationParam param);

        Task<RemoteLoginResult> RemoteLogin(string username, string password, string source);
    }
}
