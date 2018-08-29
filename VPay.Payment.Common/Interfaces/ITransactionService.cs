using System.Threading.Tasks;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Common
{
    public interface ITransactionService
    {

        Task<SecurityCheckResult> GetBalanceRequest(StandardRequest sr);
    }
}
