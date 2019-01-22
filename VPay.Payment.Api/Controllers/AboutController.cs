using Microsoft.AspNetCore.Mvc;

namespace VPay.Payment.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly AboutInfo _info;

        public AboutController(AboutInfo info)
        {
          _info = info;
        }

        /// <summary>
        /// About the build that created the service
        /// </summary>
        /// <returns>Information about the build that created the service</returns>
        /// <response code="200">Information about the build that created the service</response>
        [HttpGet]
        [ProducesResponseType(typeof(AboutInfo), 200)]
        public AboutInfo GetDetail()
        {
            return _info;
        }

    }
}
