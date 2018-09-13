using System;
using System.Threading.Tasks;

namespace VPay.Payment.Common.Db2
{
    public interface IDbPaymentOps : IDisposable
    {
        Task<string> BalanceRequest(string auth, string password, string ip, string data);
        
    }
}
