using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class TradingPostService : ITradingPostService
    {
        private readonly ILogger _logger;
        private readonly ITradingPostWs _tradingPostWs;

        public TradingPostService(IDb2Context db2Context, ILogger<TradingPostService> logger)
        {
            _logger = logger;
            _tradingPostWs = db2Context.TradingPostWs;
        }

        public async Task<TradingPostData.LoadResult> LoadCard(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.LoadRequest request)
        {
            if (auth == null)
            {
                auth = new TradingPostData.AuthenticationValuesAndIp();
            }

            _logger.LogInformation("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(LoadCard), "Starting", request.ToDisplayString());

            var validation = ValidateAuth(auth);
            validation = ValidateLoadCard(request) ?? validation;

            if (validation != null)
            {
                auth.InitialErrorCode = validation.Code;
                auth.InitialErrorMessage = validation.Message;
            }

            CleanFields(request);

            TradingPostData.LoadResult response;
            try
            {
                response = await _tradingPostWs.LoadCard(auth, request);

                if (response == null)
                {
                    response = new TradingPostData.LoadResult()
                    {
                        CardInformation = new TradingPostData.CardInformation(),
                        Claim = new TradingPostData.Claim(),
                        TransactionInformation = new TradingPostData.TransactionInformation()
                        {
                            ResponseCode = "454",
                            ResponseDescription = "Output length from procedure is 0."
                        }
                    };
                }
                else
                {
                    // duplicating this from the original code (not sure if this was deliberate or accidental)
                    response.Claim.ClaimDate = response.Claim.ClaimDeductible;
                    response.Claim.ClaimDescription = response.Claim.ClaimDeductible;
                    response.Claim.ClaimNotes = null;
                }
            }
            catch (Exception ex)
            {
                response = new TradingPostData.LoadResult()
                {
                    CardInformation = new TradingPostData.CardInformation(),
                    Claim = new TradingPostData.Claim(),
                    TransactionInformation = new TradingPostData.TransactionInformation()
                    {
                        ResponseCode = "450",
                        ResponseDescription = "Internal Error: Connection to stored procedure has failed.  This is an internal communication error."
                    }
                };
                _logger.LogError(ex, "Error running {ServiceName}", nameof(LoadCard));
            }

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ResponseCode"] = response.TransactionInformation.ResponseCode,
                ["ResponseDesc"] = response.TransactionInformation.ResponseDescription,
                ["ResultStatusCode"] = response.TransactionInformation.StatusCode,
                ["ResultStatusDesc"] = response.TransactionInformation.StatusDescription,
            }))
            {
                var level = response.TransactionInformation.ResponseCode == "00000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, "{ServiceName} - {Step} with: \n{UserRequestBody}",
                    nameof(LoadCard), "Response", response.ToDisplayString());
            }

            return response;
        }

        public async Task<TradingPostData.RetrieveResult> RetrieveCard(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.RetrieveRequest request)
        {
            if (auth == null)
            {
                auth = new TradingPostData.AuthenticationValuesAndIp();
            }

            _logger.LogInformation("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(RetrieveCard), "Starting", request.ToDisplayString());

            var validation = ValidateAuth(auth);
            validation = ValidateRetrieveCard(request) ?? validation;

            if (validation != null)
            {
                if (validation.Code != "0000")
                {
                    using (_logger.BeginScope(new Dictionary<string, object>
                    {
                        ["SuccessCode"] = validation.Code,
                        ["SuccessDesc"] = validation.Message,
                    }))
                    {
                        _logger.LogWarning("{ServiceName} - {ValidationStatus} - Message: {SuccessCode} - {SuccessDesc} \nRequest Data: {UserRequestBody}", nameof(RetrieveCard), "ValidationError", validation.Code, validation.Message, request.ToDisplayString());
                    }
                }

                auth.InitialErrorCode = validation.Code;
                auth.InitialErrorMessage = validation.Message;
            }

            TradingPostData.RetrieveResult response;
            try
            {
                response = await _tradingPostWs.RetrieveCard(auth, request);

                if (response == null)
                {
                    response = new TradingPostData.RetrieveResult()
                    {
                        CardInformation = new TradingPostData.CardInformation(),
                        Claim = new TradingPostData.Claim(),
                        TransactionInformation = new TradingPostData.TransactionInformation()
                        {
                            ResponseCode = "454",
                            ResponseDescription = "Output length from procedure is 0."
                        }
                    };
                }
                else
                {
                    // duplicating this from the original code (not sure if this was deliberate or accidental)
                    response.Claim.ClaimDate = response.Claim.ClaimDeductible;
                    response.Claim.ClaimDescription = response.Claim.ClaimDeductible;
                    response.Claim.ClaimNotes = null;
                    response.CardHolder = null;
                }
            }
            catch (Exception ex)
            {
                response = new TradingPostData.RetrieveResult()
                {
                    CardInformation = new TradingPostData.CardInformation(),
                    Claim = new TradingPostData.Claim(),
                    TransactionInformation = new TradingPostData.TransactionInformation()
                    {
                        ResponseCode = "450",
                        ResponseDescription = "Internal Error: Connection to stored procedure has failed.  This is an internal communication error."
                    }
                };
                _logger.LogError(ex, "Error running {ServiceName}", nameof(RetrieveCard));
            }

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ResponseCode"] = response.TransactionInformation.ResponseCode,
                ["ResponseDesc"] = response.TransactionInformation.ResponseDescription,
                ["ResultStatusCode"] = response.TransactionInformation.StatusCode,
                ["ResultStatusDesc"] = response.TransactionInformation.StatusDescription,
            }))
            {
                var level = response.TransactionInformation.ResponseCode == "00000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, "{ServiceName} - {Step} with: \n{UserRequestBody}",
                    nameof(RetrieveCard), "Response", response.ToDisplayString());
            }

            return response;
        }

        public async Task<TradingPostData.NotificationResult> CardNotificationRelease(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.ReleaseNotification request)
        {
            if (auth == null)
            {
                auth = new TradingPostData.AuthenticationValuesAndIp();
            }

            _logger.LogInformation("{ServiceName} - {Step} with: \n{UserRequestBody}",
                nameof(CardNotificationRelease), "Starting", request.ToDisplayString());

            var validation = ValidateAuth(auth);
            validation = ValidateNotifyCard(request) ?? validation;

            if (validation != null)
            {
                auth.InitialErrorCode = validation.Code;
                auth.InitialErrorMessage = validation.Message;
            }

            TradingPostData.NotificationResult response;
            try
            {
                response = await _tradingPostWs.CardNotificationRelease(auth, request);

                if (response == null)
                {
                    response = new TradingPostData.NotificationResult()
                    {
                        CardInformation = new TradingPostData.CardInformation(),
                        Claim = new TradingPostData.Claim(),
                        TransactionInformation = new TradingPostData.TransactionInformation()
                        {
                            ResponseCode = "454",
                            ResponseDescription = "Output length from procedure is 0."
                        }
                    };
                }
                else
                {
                    // duplicating this from the original code (not sure if this was deliberate or accidental)
                    response.Claim.ClaimDate = response.Claim.ClaimDeductible;
                    response.Claim.ClaimDescription = response.Claim.ClaimDeductible;
                    response.Claim.ClaimNotes = null;
                    response.CardHolder = null;
                }
            }
            catch (Exception ex)
            {
                response = new TradingPostData.NotificationResult()
                {
                    CardInformation = new TradingPostData.CardInformation(),
                    Claim = new TradingPostData.Claim(),
                    TransactionInformation = new TradingPostData.TransactionInformation()
                    {
                        ResponseCode = "450",
                        ResponseDescription = "Internal Error: Connection to stored procedure has failed.  This is an internal communication error."
                    }
                };
                _logger.LogError(ex, "Error running {ServiceName}", nameof(CardNotificationRelease));
            }

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ResponseCode"] = response.TransactionInformation.ResponseCode,
                ["ResponseDesc"] = response.TransactionInformation.ResponseDescription,
                ["ResultStatusCode"] = response.TransactionInformation.StatusCode,
                ["ResultStatusDesc"] = response.TransactionInformation.StatusDescription,
            }))
            {
                var level = response.TransactionInformation.ResponseCode == "00000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, "{ServiceName} - {Step} with: \n{UserRequestBody}",
                    nameof(CardNotificationRelease), "Response", response.ToDisplayString());
            }

            return response;
        }

        private void CleanFields(TradingPostData.LoadRequest request)
        {
            request.Merchant.PayeeName = request.Merchant.PayeeName.CleanWordValues();
            request.Merchant.ContactPerson = request.Merchant.ContactPerson.CleanWordValues();

            request.CoveredItem.OwnerFirstName = request.CoveredItem.OwnerFirstName.CleanWordValues();
            request.CoveredItem.OwnerLastName = request.CoveredItem.OwnerLastName.CleanWordValues();

            request.Claim.ClaimDescription = request.Claim.ClaimDescription.CleanWordValues();
            request.Claim.ClaimNotes = request.Claim.ClaimNotes.CleanWordValues();
        }

        private ValidationMessage ValidateAuth(TradingPostData.AuthenticationValuesAndIp auth)
        {
            ValidationMessage result = null;
            if (string.IsNullOrWhiteSpace(auth.UserId))
            {
                auth.UserId = "0";
                result = new ValidationMessage()
                {
                    Code = "304",
                    Message = "Authentication ID defaulted to '0'"
                };
            }

            if (string.IsNullOrWhiteSpace(auth.Password))
            {
                auth.Password = "0";
                result = new ValidationMessage()
                {
                    Code = "305",
                    Message = "Authentication password defaulted to '0'"
                };
            }

            return result;
        }

        private ValidationMessage ValidateLoadCard(TradingPostData.LoadRequest request)
        {
            ValidationMessage result = null;

            request.Claim = request.Claim ?? new TradingPostData.Claim();
            request.CoveredItem = request.CoveredItem ?? new TradingPostData.CoveredItem();

            if (string.IsNullOrWhiteSpace(request.Claim.Amount))
            {
                request.Claim.Amount = "0";
                result = new ValidationMessage()
                {
                    Code = "300",
                    Message = "Claim amount defaulted to 0"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Claim.ClaimDate) || request.Claim.ClaimDate.Length < 6)
            {
                request.Claim.ClaimDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "301",
                    Message = "Claim report date defaulted to '1000-01-01'"
                };
            }

            if (string.IsNullOrWhiteSpace(request.CoveredItem.BeginDate) || request.CoveredItem.BeginDate.Length < 6)
            {
                request.CoveredItem.BeginDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "302",
                    Message = "coveredItem begin date defaulted to '1000-01-01'"
                };
            }

            if (string.IsNullOrWhiteSpace(request.CoveredItem.ExpireDate) || request.CoveredItem.ExpireDate.Length < 6)
            {
                request.CoveredItem.ExpireDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "302",
                    Message = "coveredItem expire date defaulted to '1000-01-01'"
                };
            }

            result = ValidateDecimalAmount(request.Claim.Amount, "claim amount") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.Deductible, "item deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.BeginOdometer, "begin odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimOdometer, "claim odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimDeductible, "claim deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.ExpireOdometer, "expire odometer") ?? result;

            return result;
        }

        private ValidationMessage ValidateRetrieveCard(TradingPostData.RetrieveRequest request)
        {
            ValidationMessage result = null;

            request.CardHolder = request.CardHolder ?? new TradingPostData.Cardholder();
            request.Claim = request.Claim ?? new TradingPostData.Claim();
            request.CoveredItem = request.CoveredItem ?? new TradingPostData.CoveredItem();
            request.CardInformation = request.CardInformation ?? new TradingPostData.CardInformation();

            if (string.IsNullOrWhiteSpace(request.CardHolder.Client))
            {
                request.CardHolder.Client = " ";
                result = new ValidationMessage()
                {
                    Code = "400",
                    Message = "client defaulted to ' '"
                };
            }

            if (string.IsNullOrWhiteSpace(request.CardHolder.BillingCode))
            {
                request.CardHolder.BillingCode = " ";
                result = new ValidationMessage()
                {
                    Code = "907",
                    Message = "billing code is not valid"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Claim.UserKey))
            {
                request.Claim.UserKey = " ";
                result = new ValidationMessage()
                {
                    Code = "401",
                    Message = "userKey defaulted to ' '"
                };
            }

            if (request.CardInformation.LoadTransactionId < 0)
            {
                request.CardInformation.LoadTransactionId = 0;
                result = new ValidationMessage()
                {
                    Code = "402",
                    Message = "loadTransactionId defaulted to '0'"
                };
            }

            result = ValidateDecimalAmount(request.Claim.Amount, "claim amount") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.Deductible, "item deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.BeginOdometer, "begin odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimOdometer, "claim odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimDeductible, "claim deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.ExpireOdometer, "expire odometer") ?? result;

            return result;
        }

        private ValidationMessage ValidateNotifyCard(TradingPostData.ReleaseNotification request)
        {
            ValidationMessage result = null;

            request.Claim = request.Claim ?? new TradingPostData.Claim();
            request.CoveredItem = request.CoveredItem ?? new TradingPostData.CoveredItem();
            request.CardInformation = request.CardInformation ?? new TradingPostData.CardInformation();

            if (request.CardInformation.LoadTransactionId < 0)
            {
                request.CardInformation.LoadTransactionId = 0;
                result = new ValidationMessage()
                {
                    Code = "301",
                    Message = "defaulted loadTransactionId to 0"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Claim.Amount))
            {
                request.Claim.Amount = "0";
                result = new ValidationMessage()
                {
                    Code = "300",
                    Message = "Claim amount defaulted to 0"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Claim.ClaimDate) || request.Claim.ClaimDate.Length < 6)
            {
                request.Claim.ClaimDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "301",
                    Message = "Claim report date defaulted to '1000-01-01'"
                };
            }

            if (string.IsNullOrWhiteSpace(request.CoveredItem.BeginDate) || request.CoveredItem.BeginDate.Length < 6)
            {
                request.CoveredItem.BeginDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "301",
                    Message = "coveredItem begin date defaulted to '1000-01-01'"
                };
            }

            if (string.IsNullOrWhiteSpace(request.CoveredItem.ExpireDate) || request.CoveredItem.ExpireDate.Length < 6)
            {
                request.CoveredItem.ExpireDate = "1000-01-01";
                result = new ValidationMessage()
                {
                    Code = "302",
                    Message = "coveredItem expire date defaulted to '1000-01-01'"
                };
            }

            result = ValidateDecimalAmount(request.Claim.Amount, "claim amount") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.Deductible, "item deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.BeginOdometer, "begin odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimOdometer, "claim odometer") ?? result;

            result = ValidateDecimalAmount(request.Claim.ClaimDeductible, "claim deductible") ?? result;

            result = ValidateDecimalAmount(request.CoveredItem.ExpireOdometer, "expire odometer") ?? result;

            return result;
        }

        private ValidationMessage ValidateDecimalAmount(string amount, string fieldName)
        {
            ValidationMessage result = null;
            if (decimal.TryParse(amount, out var dec) && dec < 0)
            {
                result = new ValidationMessage()
                {
                    Code = "305",
                    Message = $"{fieldName} has been converted to a positive number"
                };
            }

            return result;
        }
    }
}
