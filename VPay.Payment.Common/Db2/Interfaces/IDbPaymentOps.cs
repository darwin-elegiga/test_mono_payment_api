using System;
using System.Threading.Tasks;

namespace VPay.Payment.Common.Db2
{
    public interface IDbPaymentOps : IDisposable
    {
        Task<string> BalanceRequest(string auth, string password, string ip, string data);
        Task<string> GetPan(string auth, string password, string ip, string data);
        Task<string> LoadPan(string auth, string password, string ip, string data);
        Task<string> OpenPreAuth(string auth, string password, string ip, string data);
        Task<string> StopPay(string auth, string password, string ip, string data);
        Task<string> Unload(string auth, string password, string ip, string data);

    }
}
