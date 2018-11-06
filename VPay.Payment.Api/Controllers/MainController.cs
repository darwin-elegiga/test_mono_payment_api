using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITransactionService _transactionService;
        
        public MainController(IHttpContextAccessor accessor, ITransactionService transactionService)
        {
            _accessor = accessor;
            _transactionService = transactionService;
        }

        [HttpGet("ReasonCodes")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public async Task<ReasonCodeResponse> GetReasonCodes(string transNumber)
        {
            var reasonCodeRequest = StandardReasonCodeRequest(transNumber);

            var reasonCodeResponse = await _transactionService.GetReasonCodes(reasonCodeRequest);

            return reasonCodeResponse;
        }

        [HttpGet("TransactionDetails")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public async Task<TransactionDetailResponse> GetTransactionDetails(string transNumber)
        {
            var transactionDetailRequest = StandardTransactionDetailRequest(transNumber);

            var transactionDetailResponse = await _transactionService.GetTransactionDetails(transactionDetailRequest);

            return transactionDetailResponse;
        }

        [HttpGet("PanNumber")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public async Task<StandardResponse> GetPanNumber(string transNumber)
        {
            var sr = DefaultStandardRequest(transNumber);

            var result = await _transactionService.GetPanNumber(sr);

            return result;
        }

        [HttpGet("OpenPreAuth")]
        [ServicePermissionAuthorize(ServicePermission.OpenPreAuth)]
        public async Task<StandardResponse> OpenPreAuth(string transNumber)
        {
            var sr = DefaultStandardRequest(transNumber);

            var result = await _transactionService.OpenPreAuth(sr);

            return result;
        }

        [HttpGet("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        public async Task<StandardResponse> LoadPan(string transNumber)
        {
            var sr = DefaultStandardRequest(transNumber);

            var result = await _transactionService.LoadPan(sr, "");

            return result;
        }

        [HttpGet("BalanceRequest")]
        [ServicePermissionAuthorize(ServicePermission.BalanceRequest)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> BalanceRequest(string transNumber)
        {
            var sr = DefaultStandardRequest(transNumber);

            var result = await _transactionService.GetBalanceRequest(sr);

            return result;
        }

        [HttpGet("UnloadPan")]
        [ServicePermissionAuthorize(ServicePermission.Unload)]
        public async Task<StandardResponse> UnloadPan(string transNumber)
        {
            var sr = StandardRequestWithUnload(transNumber);

            var result = await _transactionService.UnloadPan(sr);

            return result;
        }

        [HttpGet("StopPay")]
        [ServicePermissionAuthorize(ServicePermission.StopPay)]
        public async Task<StandardResponse> StopPay(string transNumber)
        {
            var sr = StandardRequestWithUnload(transNumber);

            var result = await _transactionService.StopPay(sr);

            return result;
        }

        [HttpPost("CancelFax")]
        [ServicePermissionAuthorize(ServicePermission.CancelFax)]
        public async Task<StandardResponse> CancelFax(FaxRequest entity)
        {
            var result = await _transactionService.CancelFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ChangeFaxNumber")]
        [ServicePermissionAuthorize(ServicePermission.ChangeFaxNumber)]
        public async Task<StandardResponse> ChangeFaxNumber(ChangeFaxNumberRequest entity)
        {
            var result = await _transactionService.ChangeFaxNumber(entity.FaxCode.GetValueOrDefault(0), entity.CleanFaxNumber);

            return result;
        }

        [HttpPost("HoldFax")]
        [ServicePermissionAuthorize(ServicePermission.HoldFax)]
        public async Task<StandardResponse> HoldFax(FaxRequest entity)
        {
            var result = await _transactionService.HoldFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ReleaseFax")]
        [ServicePermissionAuthorize(ServicePermission.ReleaseFax)]
        public async Task<StandardResponse> ReleaseFax(FaxRequest entity)
        {
            var result = await _transactionService.ReleaseFax(entity.FaxCode.GetValueOrDefault(0));

            return result;
        }

        [HttpPost("ResendFax")]
        [ServicePermissionAuthorize(ServicePermission.ResendFax)]
        public async Task<StandardResponse> ResendFax(ResendFaxRequest entity)
        {
            var result = await _transactionService.ResendFax(entity.FaxCode.GetValueOrDefault(0), entity.CleanFaxNumber);

            return result;
        }

        private StandardRequest DefaultStandardRequest(string transNumber)
        {
            var defaultStandardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    TransNumber = transNumber,
                    User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
                }
            };

            return defaultStandardRequest;
        }

        private StandardRequest StandardRequestWithUnload(string transNumber)
        {
            var defaultStandardRequest = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    TransNumber = transNumber,
                    User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
                },
                CardData = new CardData()
                {
                    UnloadCode = "4602",
                    UnloadDesc = "No Reason"
                }
            };

            return defaultStandardRequest;
        }

        private ReasonCodeRequest StandardReasonCodeRequest(string transNumber)
        {
            var reasonCodeRequest = new ReasonCodeRequest()
            {
                TransNumber = transNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
            };

            return reasonCodeRequest;
        }

        private TransactionDetailRequest StandardTransactionDetailRequest(string transNumber)
        {
            var transactionDetailRequest = new TransactionDetailRequest()
            {
                TransNumber = transNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
            };

            return transactionDetailRequest;
        }

    }
}
