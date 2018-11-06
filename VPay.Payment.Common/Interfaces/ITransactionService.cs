using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public interface ITransactionService
    {
        Task<ReasonCodeResponse> GetReasonCodes(ReasonCodeRequest request);
        Task<TransactionDetailResponse> GetTransactionDetails(TransactionDetailRequest request);
        Task<StandardResponse> GetPanNumber(StandardRequest standardRequest);
        Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest);
        Task<StandardResponse> LoadPan(StandardRequest standardRequest, string clientData);
        Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest);
        Task<StandardResponse> UnloadPan(StandardRequest standardRequest);
        Task<StandardResponse> StopPay(StandardRequest standardRequest);
        Task<StandardResponse> CancelFax(int faxCode);
        Task<StandardResponse> ChangeFaxNumber(int faxCode, string faxNumber);
        Task<StandardResponse> HoldFax(int faxCode);
        Task<StandardResponse> ReleaseFax(int faxCode);
        Task<StandardResponse> ResendFax(int faxCode, string faxNumber);

    }
}
