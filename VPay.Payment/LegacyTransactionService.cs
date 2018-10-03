using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class LegacyTransactionService : ILegacyTransactionService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILegacyValidationService _validationService;
        private readonly ILogger _logger;

        public LegacyTransactionService(ITransactionService transactionService,
            ILegacyValidationService validationService,
            ILogger<LegacyTransactionService> logger)
        {
            _transactionService = transactionService;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<StandardResponse> GetReasonCodes(ReasonCodeRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var standardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    User = request.User,
                    PassWord = request.PassWord,
                    TransNumber = request.TransNumber
                }
            };

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.GetReasonCodes(standardRequest);
        }

        public async Task<StandardResponse> GetTransactionDetails(TransactionDetailRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var standardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    User = request.User,
                    PassWord = request.PassWord,
                    TransNumber = request.Txid
                }
            };

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.GetTransactionDetails(standardRequest);
        }

        public async Task<StandardResponse> GetPanNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.GetPanNumber(standardRequest);
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.OpenPreAuth(standardRequest);
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.LoadPan(standardRequest);
        }

        public async Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.GetBalanceRequest(standardRequest);
        }

        public async Task<StandardResponse> UnloadPan(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.UnloadPan(standardRequest);
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            return await _transactionService.StopPay(standardRequest);
        }

        public async Task<StandardResponse> CancelFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.CancelFax(faxCode);
        }

        public async Task<StandardResponse> ChangeFaxNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var originalPhone = standardRequest.CorrespondenceData.PhoneNumber;
            standardRequest.CorrespondenceData.PhoneNumber = originalPhone.CleanFaxNumber();

            var validation = await _validationService.ValidateChangeFaxNumberRequest(standardRequest, originalPhone, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ChangeFaxNumber(faxCode, standardRequest.CorrespondenceData.PhoneNumber);
        }

        public async Task<StandardResponse> HoldFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.HoldFax(faxCode);
        }

        public async Task<StandardResponse> ReleaseFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ReleaseFax(faxCode);
        }

        public async Task<StandardResponse> ResendFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var originalPhone = standardRequest.CorrespondenceData.PhoneNumber;
            standardRequest.CorrespondenceData.PhoneNumber = originalPhone.CleanFaxNumber();

            var validation = await _validationService.ValidateResendFaxNumberRequest(standardRequest, originalPhone, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ResendFax(faxCode, standardRequest.CorrespondenceData.PhoneNumber);
        }
    }
}
