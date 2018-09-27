using System;
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
            SetupDefaultValuesForLoadPan(standardRequest);

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


        private void SetupDefaultValuesForLoadPan(StandardRequest sr)
        {
            if (sr.Claim != null)
            {
                if (string.IsNullOrWhiteSpace(sr.Claim.Amount))
                {
                    sr.Claim.Amount = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimOdometer))
                {
                    sr.Claim.Amount = "000000";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimDeductible))
                {
                    sr.Claim.Amount = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimDate))
                {
                    sr.Claim.Amount = "10000101";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.CurrencyType))
                {
                    sr.Claim.Amount = "USD";
                }

                if (!string.IsNullOrWhiteSpace(sr.Claim.UserField1))
                {
                    sr.Claim.UserField1 = sr.Claim.UserField1.ToUpper();
                }
            }

            if (sr.CoveredItem != null)
            {
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.ItemYear))
                {
                    sr.CoveredItem.ItemYear = "1000";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.Deductible))
                {
                    sr.CoveredItem.Deductible = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.BeginOdometer))
                {
                    sr.CoveredItem.BeginOdometer = "000000";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.ExpireDate))
                {
                    sr.CoveredItem.ExpireDate = "10000101";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.BeginDate))
                {
                    sr.CoveredItem.BeginDate = "10000101";
                }
            }

        }

        private (string code, string message) ValidateLoadPan(StandardRequest request)
        {
            var notEqualMsg = "Invalid Numeric Format:";
            var invalidDateMsg = "Date format not ISO ";
            var greaterThanAmtMsg = "Amount must be >= 0.00 ";
            var equalAmtMsg = "Amount must be = 0.00 ";

            var result = (code: "0000", message: "Successful Validation");

            if (decimal.TryParse(request.Claim.Amount, out var amount))
            {
                if (request.Payment.Type == "CLNPF")
                {
                    if (amount != 0)
                    {
                        result = (code: "0908", message: equalAmtMsg);
                    }
                }
                else
                {
                    if (amount <= 0)
                    {
                        result = (code: "0908", message: greaterThanAmtMsg);
                    }
                }
            }
            else
            {
                result = (code: "0930", message: $"{notEqualMsg} amount {request.Claim.Amount}");
            }

            if (!int.TryParse(request.CoveredItem.ItemYear, out var year))
            {
                result = (code: "0931", message: $"{notEqualMsg} year {request.CoveredItem.ItemYear}");
            }


            return result;

        }

    }
}
