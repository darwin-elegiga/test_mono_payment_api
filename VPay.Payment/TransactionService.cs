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
        private readonly IUserInfo _user;
        private readonly PaymentConfig _config;
        private readonly ILogger _logger;


        public TransactionService(IDbPaymentOps dbPaymentOps, IUserInfo user, PaymentConfig config, ILogger<TransactionService> logger)
        {
            _dbPaymentOps = dbPaymentOps;
            _user = user;
            _config = config;
            _logger = logger;
        }

        public async Task<StandardResponse> GetReasonCodes(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> GetTransactionDetails(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> GetPanNumber(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest)
        {
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.BalanceRequest("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> UnloadPan(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> CancelFax(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> ChangeFaxNumber(int faxCode, string faxNumber)
        {
            var dbResult = await _dbPaymentOps.ChangeFaxNumber(_user.Token, faxCode, faxNumber);

            var sReq = new StandardResponse()
            {
                CommonData = new CommonData()
                {
                    SuccessCode = dbResult.SuccessCode,
                    SuccessDesc = dbResult.SuccessDescription
                },
                CardData = new CardData(),
                CheckData = new CheckData(),
                Claim = new Claim(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItem(),
                Merchant = new Merchant(),
                Payment = new Common.DataWebService.Payment(),
                SwitchTransaction = new SwitchTransaction()
            };

            return sReq;
        }

        public async Task<StandardResponse> HoldFax(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> ReleaseFax(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> ResendFax(StandardRequest standardRequest)
        {
            StandardResponse resultOfISeriesCall = null;

            return resultOfISeriesCall;
        }

    }
}
