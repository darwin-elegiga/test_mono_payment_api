using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Api.Validators;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        // Add version tag
        private readonly string _version;

        private readonly IHttpContextAccessor _accessor;
        private readonly ITransactionService _transactionService;

        public LegacyController(IHttpContextAccessor accessor, ITransactionService transactionService)
        {
            _accessor = accessor;
            _transactionService = transactionService;

            // TODO:  Initialize private variables
            _version = "2018-08-10";
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public string GetVer()
        {
            return _version;
        }

        [HttpPost("echo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public Task<StandardResponse> PostEcho(EchoRequest entity)
        {
            return Task.FromResult(new StandardResponse()
            {
                CommonData = new CommonData()
                {
                    ResponseDesc = entity.Es,
                    SuccessCode = "0",
                    ReasonCode = "0"
                }
            });
        }

        [HttpPost("GetReasonCodes")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(ReasonCodeResponse), 200)]
        public async Task<ReasonCodeResponse> GetReasonCodes(LegacyRequest request)
        {
            return new ReasonCodeResponse();
        }

        [HttpPost("GetTransactionDetails")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(TransactionDetailResponse), 200)]
        public async Task<TransactionDetailResponse> GetTransactionDetails(LegacyRequest request)
        {
            return new TransactionDetailResponse();
        }

        [HttpPost("GetPanNumber")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> GetPanNumber(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.GetPanNumber?.Request);

            var validation = ValidateStandardRequest(sr, nameof(GetPanNumber));

            var result = await _transactionService.GetPanNumber(sr);

            return result;
        }

        [HttpPost("OpenPreAuth")]
        [ServicePermissionAuthorize(ServicePermission.OpenPreAuth)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> OpenPreAuth(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.OpenPreAuth?.Request);

            var validation = ValidateStandardRequest(sr, nameof(OpenPreAuth));

            var result = await _transactionService.OpenPreAuth(sr);

            return result;
        }

        [HttpPost("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> LoadPan(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.LoadPan?.Request);

            var validation = ValidateStandardRequest(sr, nameof(LoadPan));

            var result = await _transactionService.LoadPan(sr);

            return result;
        }

        [HttpPost("BalanceRequest")]
        [ServicePermissionAuthorize(ServicePermission.BalanceRequest)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> BalanceRequest(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.BalanceRequest?.Request);

            var validation = ValidateStandardRequest(sr, nameof(BalanceRequest));

            if (validation.code != "0000")
            {
                return new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessDesc = validation.message,
                        SuccessCode = validation.code
                    }
                };
            }

            var result = await _transactionService.GetBalanceRequest(sr);

            return result;
        }

        [HttpPost("UnloadPan")]
        [ServicePermissionAuthorize(ServicePermission.Unload)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> UnloadPan(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.UnloadPan?.Request);

            var validation = ValidateStandardRequest(sr, nameof(UnloadPan));

            var result = await _transactionService.UnloadPan(sr);

            return result;
        }

        [HttpPost("StopPay")]
        [ServicePermissionAuthorize(ServicePermission.StopPay)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> StopPay(LegacyRequest request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.StopPay?.Request);

            var validation = ValidateStandardRequest(sr, nameof(StopPay));

            var result = await _transactionService.StopPay(sr);

            return result;
        }

        [HttpPost("CancelFax")]
        [ServicePermissionAuthorize(ServicePermission.CancelFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> CancelFax(LegacyRequest request)
        {
            var tempRequest = request.Envelope.Body.CancelFax;

            var validation = ValidateStandardRequest(tempRequest?.Request, nameof(CancelFax));

            var entity = new FaxRequest() {};
            
            if (int.TryParse(tempRequest.Request.CorrespondenceData.FaxCode, out var faxCode))
            {
                entity.FaxCode = faxCode;
            }

            var result = await _transactionService.CancelFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ChangeFaxNumber")]
        [ServicePermissionAuthorize(ServicePermission.ChangeFaxNumber)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ChangeFaxNumber(LegacyRequest request)
        {
            var tempRequest = request.Envelope.Body.ChangeFaxNumber;

            var validation = ValidateStandardRequest(tempRequest?.Request, nameof(ChangeFaxNumber));

            var entity = new ChangeFaxNumberRequest()
            {
                FaxNumber = tempRequest.Request.CorrespondenceData.PhoneNumber
            };

            if (int.TryParse(tempRequest.Request.CorrespondenceData.FaxCode, out var faxCode))
            {
                entity.FaxCode = faxCode;
            }
            var result = await _transactionService.ChangeFaxNumber(entity.FaxCode.GetValueOrDefault(0), entity.CleanFaxNumber);

            return result;
        }

        [HttpPost("HoldFax")]
        [ServicePermissionAuthorize(ServicePermission.HoldFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> HoldFax(LegacyRequest request)
        {
            var tempRequest = request.Envelope.Body.HoldFax;

            var validation = ValidateStandardRequest(tempRequest?.Request, nameof(HoldFax));

            var entity = new FaxRequest() { };

            if (int.TryParse(tempRequest.Request.CorrespondenceData.FaxCode, out var faxCode))
            {
                entity.FaxCode = faxCode;
            }
            var result = await _transactionService.HoldFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ReleaseFax")]
        [ServicePermissionAuthorize(ServicePermission.ReleaseFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ReleaseFax(LegacyRequest request)
        {
            var tempRequest = request.Envelope.Body.ReleaseFax;

            var validation = ValidateStandardRequest(tempRequest?.Request, nameof(ReleaseFax));

            var entity = new FaxRequest() { };

            if (int.TryParse(tempRequest.Request.CorrespondenceData.FaxCode, out var faxCode))
            {
                entity.FaxCode = faxCode;
            }
            var result = await _transactionService.ReleaseFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ResendFax")]
        [ServicePermissionAuthorize(ServicePermission.ResendFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ResendFax(LegacyRequest request)
        {
            var tempRequest = request.Envelope.Body.ResendFax;

            var validation = ValidateStandardRequest(tempRequest?.Request, nameof(ResendFax));

            var entity = new ChangeFaxNumberRequest()
            {
                FaxNumber = tempRequest.Request.CorrespondenceData.PhoneNumber
            };

            if (int.TryParse(tempRequest.Request.CorrespondenceData.FaxCode, out var faxCode))
            {
                entity.FaxCode = faxCode;
            }
            var result = await _transactionService.ResendFax(entity.FaxCode.GetValueOrDefault(0), entity.CleanFaxNumber);

            return result;
        }

        private StandardRequest DefaultStandardRequest(StandardRequest request)
        {
            request.CommonData.User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            request.CommonData.Token =
                _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return request;
        }

        private (string code, string message) ValidateStandardRequest(StandardRequest sr, string methodName)
        {
            var result = (code: "0000", message: "Successful Validation");

            var ruleSets = string.IsNullOrWhiteSpace(methodName) ? "default" : $"default,{methodName}";

            var val = (new CardDataValidator()).Validate(sr.CardData, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new CheckDataValidator()).Validate(sr.CheckData, ruleSet: ruleSets);
            
            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new ClaimDataValidator()).Validate(sr.Claim, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new CommonDataValidator()).Validate(sr.CommonData, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new CorrespondenceDataValidator()).Validate(sr.CorrespondenceData, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new CoveredItemDataValidator()).Validate(sr.CoveredItem, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new MerchantDataValidator()).Validate(sr.Merchant, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new PaymentDataValidator()).Validate(sr.Payment, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            val = (new SwitchTransactionDataValidator()).Validate(sr.SwitchTransaction, ruleSet: ruleSets);

            if (!val.IsValid)
            {
                return CombineErrorMessages(val.Errors);
            }

            return result;
        }

        private (string code, string message) CombineErrorMessages(IEnumerable<ValidationFailure> validationErrors)
        {
            var fieldsToLong = new List<string>();
            foreach (var error in validationErrors.GroupBy(x => x.ErrorCode))
            {
                if (error.Key == "0990")
                {
                    fieldsToLong.AddRange(error.Select(x => x.PropertyName.FirstCharacterToLower()));
                }

                if (error.Key == "0055")
                {
                    return (code: error.Key, message: error.First().ErrorMessage);
                }

                if (error.Key == "0005")
                {
                    return (code: error.Key, message: error.First().ErrorMessage);
                }
            }

            if (fieldsToLong.Any())
            {
                return (code: "0990", message: $"Fields too long: {string.Join(", ", fieldsToLong)}");
            }

            return (code: "9999", message: "Unknown Error");
        }

    }
}
