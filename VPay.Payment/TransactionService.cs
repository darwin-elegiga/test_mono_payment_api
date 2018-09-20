using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
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
        private readonly IDb2Context _db2Context;
        private readonly IUserInfo _user;
        private readonly PaymentConfig _config;
        private readonly ILogger _logger;


        public TransactionService(IDbPaymentOps dbPaymentOps, IDb2Context db2Context, IUserInfo user, PaymentConfig config, ILogger<TransactionService> logger)
        {
            _dbPaymentOps = dbPaymentOps;
            _db2Context = db2Context;
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
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.GetPan("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest)
        {
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.OpenPreAuth("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest)
        {
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.LoadPan("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

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
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.Unload("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest)
        {
            ServiceString serviceStringHelper = new ServiceString(_config.UseCheckEmail, standardRequest);

            string textResultOfDb2Call = await _dbPaymentOps.StopPay("WSQATEST", "QATEST01WS18", "10.120.202.129",
                serviceStringHelper.GenerateStringForISeriesCall());

            StandardResponse resultOfISeriesCall = ServiceString.ParseToStandardResponse(_config.UseCheckEmail, textResultOfDb2Call);

            return resultOfISeriesCall;
        }

        public async Task<StandardResponse> CancelFax(int faxCode)
        {
            var dbResult = await _db2Context.Fax.CancelFaxAsync(_user.Token, faxCode);

            var sResp = new StandardResponse()
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

            return sResp;
        }

        public async Task<StandardResponse> ChangeFaxNumber(int faxCode, string faxNumber)
        {
            var dbResult = await _db2Context.Fax.ChangeFaxNumberAsync(_user.Token, faxCode, faxNumber);

            var sResp = new StandardResponse()
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

            return sResp;
        }

        public async Task<StandardResponse> HoldFax(int faxCode)
        {
            var dbResult = await _db2Context.Fax.HoldFaxAsync(_user.Token, faxCode);

            var sResp = new StandardResponse()
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

            return sResp;
        }

        public async Task<StandardResponse> ReleaseFax(int faxCode)
        {
            var dbResult = await _db2Context.Fax.ReleaseFaxAsync(_user.Token, faxCode);

            var sResp = new StandardResponse()
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

            return sResp;
        }

        public async Task<StandardResponse> ResendFax(int faxCode, string faxNumber)
        {
            var dbResult = await _db2Context.Fax.ResendFaxAsync(_user.Token, faxCode, faxNumber ?? "");

            var sResp = new StandardResponse()
            {
                CommonData = new CommonData()
                {
                    SuccessCode = dbResult.SuccessCode,
                    SuccessDesc = dbResult.SuccessDescription
                },
                CardData = new CardData(),
                CheckData = new CheckData(),
                Claim = new Claim(),
                CorrespondenceData = new CorrespondenceData()
                {
                    PhoneNumber = dbResult.FaxNumber
                },
                CoveredItem = new CoveredItem(),
                Merchant = new Merchant(),
                Payment = new Common.DataWebService.Payment(),
                SwitchTransaction = new SwitchTransaction()
            };

            return sResp;
        }

    }
}
