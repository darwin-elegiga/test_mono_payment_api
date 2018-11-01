using System.Threading;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public interface ILegacyValidationService
    {
        Task<ValidationMessage> ValidateStandardRequest(StandardRequest standardRequest,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationMessage> ValidateChangeFaxNumberRequest(StandardRequest standardRequest,
            string originalFaxNumber,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationMessage> ValidateResendFaxNumberRequest(StandardRequest standardRequest,
            string originalFaxNumber,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
