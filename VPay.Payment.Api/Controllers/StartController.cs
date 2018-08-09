using Microsoft.AspNetCore.Mvc;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
        }
    }
}