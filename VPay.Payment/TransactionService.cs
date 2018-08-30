using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Payment.Common;
using VPay.Payment.Common.CommunicationStrings;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;
using VPay.Payment.Common.Models;

namespace VPay.Payment
{
    public class TransactionService : ITransactionService
    {

        private readonly IDbPaymentOps _dbPaymentOps;
        private readonly PaymentConfig _config;
        private readonly ILogger _logger;


        public TransactionService(IDbPaymentOps dbPaymentOps, PaymentConfig config, ILogger<TransactionService> logger)
        {
            _dbPaymentOps = dbPaymentOps;
            _config = config;
            _logger = logger;
        }


        public async Task<StandardResponse> GetBalanceRequest(StandardRequest sr)
        {
            var stringManipule = new ServiceString(_config.UseCheckEmail, sr);

            var dbResult = await _dbPaymentOps.BalanceRequest("WSQATEST", "QATEST01WS18", "10.120.202.129",
                stringManipule.GenerateStringForISeriesCall());

            var result = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, dbResult);

            return result;
        }


    }
}
