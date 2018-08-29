using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Payment.Common;
using VPay.Payment.Common.CommunicationStrings;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;

namespace VPay.Payment
{
    public class TransactionService : ITransactionService
    {

        private readonly IDbPaymentOps _dbPaymentOps;
        private readonly ILogger _logger;


        public TransactionService(IDbPaymentOps dbPaymentOps, ILogger<TransactionService> logger)
        {
            _dbPaymentOps = dbPaymentOps;
            _logger = logger;
        }


        public async Task<StandardResponse> GetBalanceRequest(StandardRequest sr)
        {
            var stringManipule = new ServiceString(sr);

            var dbResult = await _dbPaymentOps.BalanceRequest("WSQATEST", "QATEST01WS18", "10.120.202.129",
                stringManipule.GenerateStringForISeriesCall());

            var result = ServiceString.ParseToStandardResponse(dbResult);

            return result;
        }


    }
}
