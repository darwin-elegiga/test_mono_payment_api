using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public Task<string> Echo()
        {
            return Task.FromResult("echo");
        }

        [HttpGet("ReasonCodes")]
        public Task<string> GetReasonCodes()
        {
            return Task.FromResult("ReasonCodes");
        }

        [HttpGet("TransactionDetails")]
        public Task<string> GetTransactionDetails()
        {
            return Task.FromResult("GetTransactionDetails");
        }

        //public Task GetPanNumber()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task OpenPreAuth()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task LoadPan()
        //{
        //    return Task.CompletedTask;
        //}

        [HttpGet("BalanceRequest")]
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

        //public Task UnloadPan()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task StopPay()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task CancelFax()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task ChangeFaxNumber()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task HoldFax()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task ReleaseFax()
        //{
        //    return Task.CompletedTask;
        //}

        //public Task ResendFax()
        //{
        //    return Task.CompletedTask;
        //}

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
