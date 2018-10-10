using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
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

        public async Task<ReasonCodeResponse> GetReasonCodes(ReasonCodeRequest request, string token)
        {
            var reasonCodeResponse = ReasonCodeAdvancedLogic(request, token, _dbPaymentOps);

            return reasonCodeResponse;
        }

        public async Task<TransactionDetailResponse> GetTransactionDetails(TransactionDetailRequest request, StandardRequest standardRequest, string token)
        {
            var result = TransactionDetailAdvancedLogic(request, standardRequest, token, _dbPaymentOps);

            return result;
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

        public ReasonCodeResponse ReasonCodeAdvancedLogic(ReasonCodeRequest request, string token, IDbPaymentOps dbPaymentOps)
        {
            List<ReasonCodeType> reasonCodes = dbPaymentOps.ReasonCodesData(token, request.User, request.TransNumber).Result;
            ReasonCodeResponse theResponse = new ReasonCodeResponse();
            theResponse.ReasonCodeList = reasonCodes;
            theResponse.CommonData = new CommonData() { SuccessCode = "0000", SuccessDesc = "Authorized" };

            return theResponse;
        }

        public TransactionDetailResponse TransactionDetailAdvancedLogic(TransactionDetailRequest request, StandardRequest standardRequest, string token, IDbPaymentOps dbPaymentOps)
        {
            AuthenticationValues standardAuthenticationValues = new AuthenticationValues("WSQATEST", "QATEST01WS18");
            var headerDatas = dbPaymentOps.TransactionHeadersData(token, request.User, request.PassWord, request.Txid,
                standardAuthenticationValues, "10.120.202.129").Result;
            List<HeaderData> waitedHeaderDatas = headerDatas;
            string client = waitedHeaderDatas[0].Client;
            string billCode = waitedHeaderDatas[0].BillCode;

            StandardResponse balRequestResponse = GetBalanceRequest(standardRequest).Result;
            headerDatas[0].SwitchAvailBal = balRequestResponse.SwitchTransaction.AvailableBal;
            headerDatas[0].SwitchCurrentBal = balRequestResponse.SwitchTransaction.CurrentBal;

            StandardResponse panNumResponse = GetPanNumber(standardRequest).Result;
            PayTypeDetail payTypeDetail = new PayTypeDetail();
            payTypeDetail.CardNumber = panNumResponse.CardData.CardNumber;
            payTypeDetail.CardCvv2 = panNumResponse.CardData.CardCvv2;
            payTypeDetail.CardExp = panNumResponse.CardData.CardExpiration;
            payTypeDetail.Association = panNumResponse.CardData.CardType;
            payTypeDetail.Bank = panNumResponse.CardData.CardholderName;
            payTypeDetail.OutsideCheck = panNumResponse.CheckData.CheckNumber;
            payTypeDetail.PosPayCheck = panNumResponse.CheckData.PosPayNumber;
            payTypeDetail.SwitchNumber = panNumResponse.CheckData.SwitchNumber;
            payTypeDetail.ClearCheck = panNumResponse.CheckData.ChkNum1;

            var detailList = dbPaymentOps.TransactionDetailsData(token, client, billCode, request.Txid).Result;
            var correspList = dbPaymentOps.TransactionCorrespondenceData(token, request.User, request.Txid).Result;

            var returnValue = new TransactionDetailResponse()
            {
                DetailList = detailList,
                HeaderData = headerDatas[0],
                CorrespondenceList = correspList,
                PayTypeDetail = payTypeDetail,
                CommonData = new CommonData()
                {
                    User = request.User.ToUpper(),
                    TransNumber = request.Txid,
                    SuccessCode = "0000",
                    SuccessDesc = "Successful Query"
                }
            };

            return returnValue;
        }

    }
}
