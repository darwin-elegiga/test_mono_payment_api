using System.Threading.Tasks;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Common
{
    public interface ITransactionService
    {

        Task<StandardResponse> GetBalanceRequest(StandardRequest sr);
    }
}
