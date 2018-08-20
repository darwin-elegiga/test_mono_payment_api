using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VPay.Payment.Api.Auth;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        public MainController()
        {
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
        // WebService ISeries Pool
        private WSiPool wsi;
        // Add version tag
        private string version;

        [HttpGet("echo")]
        [AllowAnonymous]
        public Task<string> Echo()
        {
            return Task.FromResult("echo");
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
        public Task<StandardResponse> BalanceRequest()
        {
            AuthenticationValues av = new AuthenticationValues();
            StandardRequest sr = new StandardRequest();

            string webSvc = "BALREQUEST";
            string svcName = "BalRequest";
            string action = "READ";
            string secGrp = "WSPUBLIC";

            StandardResponse returnValue = Run(av, sr, webSvc, svcName, secGrp, action);

            return Task.FromResult(returnValue);
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

        [HttpGet("CancelFax")]
        [ServicePermissionAuthorize(ServicePermission.CancelFax)]
        public Task<string> CancelFax()
        {
            return Task.FromResult("CancelFax");
        }

        [HttpGet("ChangeFaxNumber")]
        [ServicePermissionAuthorize(ServicePermission.ChangeFaxNumber)]
        public Task<string> ChangeFaxNumber()
        {
            return Task.FromResult("ChangeFaxNumber");
        }

        [HttpGet("HoldFax")]
        [ServicePermissionAuthorize(ServicePermission.HoldFax)]
        public Task<string> HoldFax()
        {
            return Task.FromResult("HoldFax");
        }

        [HttpGet("ReleaseFax")]
        [ServicePermissionAuthorize(ServicePermission.ReleaseFax)]
        public Task<string> ReleaseFax()
        {
            return Task.FromResult("ReleaseFax");
        }

        [HttpGet("ResendFax")]
        [ServicePermissionAuthorize(ServicePermission.ResendFax)]
        public Task<string> ResendFax()
        {
            return Task.FromResult("ResendFax");
        }

        private StandardResponse Run(AuthenticationValues av, StandardRequest sr, string webSvc, string svcName,
            string secGrp, string action, CustomData ct)
        {
            return null;
        }

        private StandardResponse Run(AuthenticationValues av, StandardRequest sr, string webSvc, string svcName,
            string secGrp, string action)
        {
            return Run(av, sr, webSvc, svcName, secGrp, action, new CustomData());
        }

    }
}
