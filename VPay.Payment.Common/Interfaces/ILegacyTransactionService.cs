using System.Threading;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public interface ILegacyTransactionService
    {
        Task<StandardResponse> GetReasonCodes(ReasonCodeRequest request, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> GetTransactionDetails(TransactionDetailRequest request, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> GetPanNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> LoadPan(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> UnloadPan(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> StopPay(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> CancelFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> ChangeFaxNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> HoldFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> ReleaseFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));
        Task<StandardResponse> ResendFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken));

    }
}
