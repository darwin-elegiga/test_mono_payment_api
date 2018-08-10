using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VPay.Payment.Common;
using VPay.Payment.Common.Models;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private readonly IEnumerable<IHealthCheck> _dbCheck;

        public StartController(IEnumerable<IHealthCheck> dbCheck)
        {
            _dbCheck = dbCheck;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
        }

        [HttpGet("db2-status")]
        public async Task<ActionResult<IEnumerable<ServiceComponentStatus>>> GetDb2Status()
        {
            var results = new List<ServiceComponentStatus>();

            foreach (var check in _dbCheck)
            {
                results.Add(new ServiceComponentStatus()
                {
                    Component = check.Component,
                    Status = (await check.IsHealthy()).ToString()
                });
            }

            return results;
        }
    }
}
