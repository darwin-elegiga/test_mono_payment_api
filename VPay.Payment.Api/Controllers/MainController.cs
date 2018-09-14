using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;

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
        [AllowAnonymous]
        [Produces("application/json")]
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

        [HttpGet("ReasonCodes")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public Task<string> GetReasonCodes()
        {
            return Task.FromResult("ReasonCodes");
        }

        [HttpGet("TransactionDetails")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public Task<string> GetTransactionDetails()
        {
            return Task.FromResult("GetTransactionDetails");
        }

        [HttpGet("PanNumber")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        public Task<string> GetPanNumber()
        {
            return Task.FromResult("GetPanNumber");
        }

        [HttpGet("OpenPreAuth")]
        [ServicePermissionAuthorize(ServicePermission.OpenPreAuth)]
        public Task<string> OpenPreAuth()
        {
            return Task.FromResult("OpenPreAuth");
        }

        [HttpGet("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        public Task<string> LoadPan()
        {
            return Task.FromResult("LoadPan");
        }

        [HttpGet("BalanceRequest")]
        [ServicePermissionAuthorize(ServicePermission.BalanceRequest)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> BalanceRequest(string transNumber)
        {
            var sr = new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    TransNumber = transNumber,
                    User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    PassWord = "yraheem197"
                }
            };

            var result = await _transactionService.GetBalanceRequest(sr);

            return result;
        }

        [HttpGet("UnloadPan")]
        [ServicePermissionAuthorize(ServicePermission.Unload)]
        public Task<string> UnloadPan()
        {
            return Task.FromResult("UnloadPan");
        }

        [HttpGet("StopPay")]
        [ServicePermissionAuthorize(ServicePermission.StopPay)]
        public Task<string> StopPay()
        {
            return Task.FromResult("StopPay");
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

    }
}
