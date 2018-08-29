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


        public async Task<SecurityCheckResult> GetBalanceRequest(StandardRequest sr)
        {
            var stringManipule = new ServiceString(
                sr.CommonData ?? new CommonData(),
                sr.CardData ?? new CardData(),
                sr.CheckData ?? new CheckData(),
                sr.Claim ?? new Claim(),
                sr.CorrespondenceData ?? new CorrespondenceData(),
                sr.CoveredItem ?? new CoveredItem(),
                sr.Merchant ?? new Merchant(),
                sr.Payment ?? new Common.DataWebService.Payment(),
                sr.SwitchTransaction ?? new SwitchTransaction());


            var result = await _dbPaymentOps.BalanceRequest("WSQATEST", "QATEST01WS18", "10.120.202.129",
                stringManipule.GenerateStringForISeriesCall());

            return result;
        }


    }
}
