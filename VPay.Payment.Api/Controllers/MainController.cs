using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VPay.Payment.Common;

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

        [HttpGet]
        public Task Echo()
        {
            return Task.CompletedTask;
        }

        [HttpGet]
        public Task GetReasonCodes()
        {
            return Task.CompletedTask;
        }

        public Task GetTransactionDetails()
        {
            return Task.CompletedTask;
        }

        public Task GetPanNumber()
        {
            return Task.CompletedTask;
        }

        public Task OpenPreAuth()
        {
            return Task.CompletedTask;
        }

        public Task LoadPan()
        {
            return Task.CompletedTask;
        }

        public Task BalanceRequest()
        {
            return Task.CompletedTask;
        }

        public Task UnloadPan()
        {
            return Task.CompletedTask;
        }

        public Task StopPay()
        {
            return Task.CompletedTask;
        }

        public Task CancelFax()
        {
            return Task.CompletedTask;
        }

        public Task ChangeFaxNumber()
        {
            return Task.CompletedTask;
        }

        public Task HoldFax()
        {
            return Task.CompletedTask;
        }

        public Task ReleaseFax()
        {
            return Task.CompletedTask;
        }

        public Task ResendFax()
        {
            return Task.CompletedTask;
        }
    }
}
