using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITransactionService _transactionService;

        public LegacyController(IHttpContextAccessor accessor, ITransactionService transactionService)
        {
            _accessor = accessor;
            _transactionService = transactionService;

            // TODO:  Initialize private variables
            version = "2018-08-10";
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public string GetVer()
        {
            return version;
        }

        // private WebServiceContext wsContext;
        // Common Utilities
        // private VPayWSBase vbase;
        // Validation object
        // private ValidateParm vparm;
        // Logging Object
        private ILogger li;

        // Add version tag
        private string version;

        [HttpPost("echo")]
        [ProducesResponseType(typeof(CommonData), 200)]
        public Task<CommonData> PostEcho(EchoRequest entity)
        {
            return Task.FromResult(new CommonData()
            {
                ResponseDesc = entity.Es,
                SuccessCode = "0",
                ReasonCode = "0"
            });
        }

        [HttpPost("GetReasonCodes")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(ReasonCodeResponse), 200)]
        public async Task<ReasonCodeResponse> GetReasonCodes(ReasonCodeRequest reasonCodeRequest)
        {
            return new ReasonCodeResponse();
        }

        [HttpPost("GetTransactionDetails")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(TransactionDetailResponse), 200)]
        public async Task<TransactionDetailResponse> GetTransactionDetails(TransactionDetailRequest transactionDetailRequest)
        {
            return new TransactionDetailResponse();
        }

        [HttpPost("GetPanNumber")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> GetPanNumber(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.GetPanNumber?.Request);

            var result = await _transactionService.GetPanNumber(sr);

            return result;
        }

        [HttpPost("OpenPreAuth")]
        [ServicePermissionAuthorize(ServicePermission.OpenPreAuth)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> OpenPreAuth(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.OpenPreAuth?.Request);

            var result = await _transactionService.OpenPreAuth(sr);

            return result;
        }

        [HttpPost("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> LoadPan(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.LoadPan?.Request);

            var result = await _transactionService.LoadPan(sr);

            return result;
        }

        [HttpPost("BalanceRequest")]
        [ServicePermissionAuthorize(ServicePermission.BalanceRequest)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> BalanceRequest(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.BalanceRequest?.Request);

            var result = await _transactionService.GetBalanceRequest(sr);

            return result;
        }

        [HttpPost("UnloadPan")]
        [ServicePermissionAuthorize(ServicePermission.Unload)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> UnloadPan(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.UnloadPan?.Request);

            var result = await _transactionService.UnloadPan(sr);

            return result;
        }

        [HttpPost("StopPay")]
        [ServicePermissionAuthorize(ServicePermission.StopPay)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> StopPay(LegacyEnvelopeDto request)
        {
            var sr = DefaultStandardRequest(request.Envelope.Body.StopPay?.Request);

            var result = await _transactionService.StopPay(sr);

            return result;
        }

        [HttpPost("CancelFax")]
        [ServicePermissionAuthorize(ServicePermission.CancelFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> CancelFax(LegacyEnvelopeDto request)
        {
            var tempRequest = request.Envelope.Body.CancelFax;
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
        public async Task<StandardResponse> ChangeFaxNumber(LegacyEnvelopeDto request)
        {
            var tempRequest = request.Envelope.Body.ChangeFaxNumber;
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
        public async Task<StandardResponse> HoldFax(LegacyEnvelopeDto request)
        {
            var tempRequest = request.Envelope.Body.HoldFax;
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
        public async Task<StandardResponse> ReleaseFax(LegacyEnvelopeDto request)
        {
            var tempRequest = request.Envelope.Body.ReleaseFax;
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
        public async Task<StandardResponse> ResendFax(LegacyEnvelopeDto request)
        {
            var tempRequest = request.Envelope.Body.ResendFax;
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
    }
}
