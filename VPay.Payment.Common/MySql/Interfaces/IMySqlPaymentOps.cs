using System;
using System.Threading.Tasks;
using VPay.Payment.Common.Permissions;

namespace VPay.Payment.Common.MySql
{
    public interface IMySqlPaymentOps : IDisposable
    {
        Task<SessionEntry> GetSessionEntryBySessionId(string sessionId);
        Task<bool> InsertSessionEntry(SessionEntry entity);

        Task<Webucf> GetWebUfcByUserName(string userName);
    }
}
