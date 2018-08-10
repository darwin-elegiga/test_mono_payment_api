using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private readonly IHealthCheck _dbCheck;

        public StartController(IHealthCheck dbCheck)
        {
            _dbCheck = dbCheck;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
        }

        [HttpGet("db2-status")]
        public async Task<ActionResult<bool>> GetDb2Status()
        {
            return await _dbCheck.IsHealthy();
        }
    }
}
