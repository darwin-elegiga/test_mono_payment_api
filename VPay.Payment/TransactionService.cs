using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
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
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.GetPan(request);

            return result;
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest)
        {
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.OpenPreAuth(request);

            return result;
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest)
        {
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.LoadPan(request);

            return result;
        }

        public async Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest)
        {
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.BalanceRequest(request);

            return result;
        }

        public async Task<StandardResponse> UnloadPan(StandardRequest standardRequest)
        {
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.UnloadPan(request);
            return result;
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest)
        {
            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.StopPay(request);

            return result;
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
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
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
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
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
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
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
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
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
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData()
                {
                    PhoneNumber = dbResult.FaxNumber
                },
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
            };

            return sResp;
        }

    }
}
