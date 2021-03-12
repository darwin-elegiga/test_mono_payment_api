using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
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

        public async Task<ReasonCodeResponse> GetReasonCodes(ReasonCodeRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var standardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    User = request.User,
                    Token = request.Token,
                    TransNumber = request.TransNumber
                },
                Source = request.Source
            };

            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(GetReasonCodes), "Validating", standardRequest.ToDisplayString());

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(GetReasonCodes), "ValidationError", standardRequest.ToDisplayString());
                }

                return new ReasonCodeResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(GetReasonCodes), "Valid");

            return await _transactionService.GetReasonCodes(request);
        }

        public async Task<TransactionDetailResponse> GetTransactionDetails(TransactionDetailRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var standardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    User = request.User,
                    Token = request.Token,
                    TransNumber = request.TransNumber
                },
                Source = request.Source
            };

            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(GetTransactionDetails), "Validating", standardRequest.ToDisplayString());

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(GetTransactionDetails), "ValidationError", standardRequest.ToDisplayString());
                }

                return new TransactionDetailResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(GetTransactionDetails), "Valid");

            return await _transactionService.GetTransactionDetails(request);
        }

        public async Task<StandardResponse> GetPanNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(GetPanNumber), "Validating", standardRequest.ToDisplayString());

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(GetPanNumber), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(GetPanNumber), "Valid");

            return await _transactionService.GetPanNumber(standardRequest);
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(OpenPreAuth), "Validating", standardRequest.ToDisplayString());
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(OpenPreAuth), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(OpenPreAuth), "Valid");

            return await _transactionService.OpenPreAuth(standardRequest);
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest, string clientData, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}\nCustomData: {clientData}",
                nameof(LoadPan), "TruncatingData", standardRequest.ToDisplayString(), clientData);

            CleanFields(standardRequest);

            TruncateFieldsForLoadPan(standardRequest);

            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}\nCustomData: {clientData}",
                nameof(LoadPan), "Validating", standardRequest.ToDisplayString(), clientData);

            var validation = await _validationService.ValidateLoadPanStandardRequest(standardRequest, clientData, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(LoadPan), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogInformation("{ServiceName} - {ValidationStatus}",
                nameof(LoadPan), "Valid");

            return await _transactionService.LoadPan(standardRequest, clientData);
        }

        public async Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(GetBalanceRequest), "Validating", standardRequest.ToDisplayString());
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(GetBalanceRequest), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(GetBalanceRequest), "Valid");

            return await _transactionService.GetBalanceRequest(standardRequest);
        }

        public async Task<StandardResponse> UnloadPan(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(UnloadPan), "Validating", standardRequest.ToDisplayString());
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(UnloadPan), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(UnloadPan), "Valid");

            return await _transactionService.UnloadPan(standardRequest);
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(StopPay), "Validating", standardRequest.ToDisplayString());
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(StopPay), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(StopPay), "Valid");

            return await _transactionService.StopPay(standardRequest);
        }

        public async Task<StandardResponse> CancelFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(CancelFax), "Validating", standardRequest.ToDisplayString());
            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(CancelFax), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(CancelFax), "Valid");

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.CancelFax(faxCode);
        }

        public async Task<StandardResponse> ChangeFaxNumber(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}",
                nameof(ChangeFaxNumber), "Validating");

            var originalPhone = standardRequest.CorrespondenceData.PhoneNumber;
            standardRequest.CorrespondenceData.PhoneNumber = originalPhone.CleanFaxNumber();

            var validation = await _validationService.ValidateChangeFaxNumberRequest(standardRequest, originalPhone, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(ChangeFaxNumber), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(ChangeFaxNumber), "Valid");

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ChangeFaxNumber(faxCode, standardRequest.CorrespondenceData.PhoneNumber);
        }

        public async Task<StandardResponse> HoldFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(HoldFax), "Validating", standardRequest.ToDisplayString());

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(HoldFax), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(HoldFax), "Valid");

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.HoldFax(faxCode);
        }

        public async Task<StandardResponse> ReleaseFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(ReleaseFax), "Validating", standardRequest.ToDisplayString());

            var validation = await _validationService.ValidateStandardRequest(standardRequest, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(ReleaseFax), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(ReleaseFax), "Valid");

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ReleaseFax(faxCode);
        }

        public async Task<StandardResponse> ResendFax(StandardRequest standardRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogDebug("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(ResendFax), "Validating", standardRequest.ToDisplayString());

            var originalPhone = standardRequest.CorrespondenceData.PhoneNumber;
            standardRequest.CorrespondenceData.PhoneNumber = originalPhone.CleanFaxNumber();

            var validation = await _validationService.ValidateResendFaxNumberRequest(standardRequest, originalPhone, cancellationToken);

            if (validation != null && validation.Code != "0000")
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    ["SuccessCode"] = validation.Code,
                    ["SuccessDesc"] = validation.Message,
                }))
                {
                    _logger.LogWarning("{ServiceName} - {ValidationStatus} - Original Message: {UserRequestBody}",
                        nameof(ResendFax), "ValidationError", standardRequest.ToDisplayString());
                }

                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.Message,
                        SuccessCode = validation.Code
                    }
                };
            }

            _logger.LogDebug("{ServiceName} - {ValidationStatus}",
                nameof(ResendFax), "Valid");

            if (int.TryParse(standardRequest.CorrespondenceData.FaxCode, out var faxCode))
            {
            }

            return await _transactionService.ResendFax(faxCode, standardRequest.CorrespondenceData.PhoneNumber);
        }

        #region Private fields

        private void CleanFields(StandardRequest standardRequest)
        {
            standardRequest.Merchant.PayeeName = standardRequest.Merchant.PayeeName.CleanWordValues();
            standardRequest.Merchant.ContactPerson = standardRequest.Merchant.ContactPerson.CleanWordValues();

            standardRequest.CoveredItem.OwnerFirstName = standardRequest.CoveredItem.OwnerFirstName.CleanWordValues();
            standardRequest.CoveredItem.OwnerLastName = standardRequest.CoveredItem.OwnerLastName.CleanWordValues();

            standardRequest.Claim.ClaimDescription = standardRequest.Claim.ClaimDescription.CleanWordValues();
            standardRequest.Claim.ClaimNotes = standardRequest.Claim.ClaimNotes.CleanWordValues();
        }

        /// <summary>
        /// Sets up and calls truncation for fields and entities to truncate to the Maximum allowed length for that field.
        /// This one is meant only for the LoadPan at this point in time.
        /// </summary>
        /// <param name="standardRequest"></param>
        private void TruncateFieldsForLoadPan(StandardRequest standardRequest)
        {
            var merchantFieldsToTruncate = new List<string>
            {
                nameof(standardRequest.Merchant.PayeeName),
                nameof(standardRequest.Merchant.ContactPerson)
            };

            var coverItemFieldsToTruncate = new List<string>()
            {
                nameof(standardRequest.CoveredItem.OwnerFirstName),
                nameof(standardRequest.CoveredItem.OwnerLastName)
            };

            TruncateFieldsToMaxLengthForEntity(standardRequest.Merchant, merchantFieldsToTruncate);
            TruncateFieldsToMaxLengthForEntity(standardRequest.CoveredItem, coverItemFieldsToTruncate);
        }

        /// <summary>
        /// Loop through the fields on the entity and check if it past its length and the property is one that we want to truncate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldsToTruncate"></param>
        private void TruncateFieldsToMaxLengthForEntity<T>(T entity, List<string> fieldsToTruncate)
        {
            if (entity == null || fieldsToTruncate == null || fieldsToTruncate.Count == 0)
            {
                return;
            }

            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(T)))
            {
                var value = (string)prop.GetValue(entity);

                if (value != null && value.Length > attr.Length && fieldsToTruncate.Contains(prop.Name))
                {
                    prop.SetValue(entity, value.Substring(0, attr.Length));
                }
            }
        }

        #endregion Private fields
    }
}
