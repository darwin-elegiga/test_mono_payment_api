using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }


        /// <summary>
        /// Checks to see if the application can connect to all the required services
        /// </summary>
        /// <returns>This returns the services needed and what their current status is</returns>
        /// <response code="200">Returns if every service passes it's health check</response>
        /// <response code="500">Returns if at least one service fails it's health check</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceComponentStatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ServiceComponentStatus>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ServiceComponentStatus>>> CheckHealth()
        {
            var result = (await _healthCheckService.CheckHealth()).ToList();
            var serviceOk = true;
            foreach (var svc in result)
            {
                if (svc.Status != "OK") { serviceOk = false; }
            }

            if (serviceOk)
            {
                return result.ToList();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

    }
}
