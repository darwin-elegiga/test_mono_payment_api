using System.Threading.Tasks;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Common
{
    public interface ITransactionService
    {
        Task<StandardResponse> GetReasonCodes(StandardRequest standardRequest);
        Task<StandardResponse> GetTransactionDetails(StandardRequest standardRequest);
        Task<StandardResponse> GetPanNumber(StandardRequest standardRequest);
        Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest);
        Task<StandardResponse> LoadPan(StandardRequest standardRequest);
        Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest);
        Task<StandardResponse> UnloadPan(StandardRequest standardRequest);
        Task<StandardResponse> StopPay(StandardRequest standardRequest);
        Task<StandardResponse> CancelFax(StandardRequest standardRequest);
        Task<StandardResponse> ChangeFaxNumber(StandardRequest standardRequest);
        Task<StandardResponse> HoldFax(StandardRequest standardRequest);
        Task<StandardResponse> ReleaseFax(StandardRequest standardRequest);
        Task<StandardResponse> ResendFax(StandardRequest standardRequest);

    }
}
