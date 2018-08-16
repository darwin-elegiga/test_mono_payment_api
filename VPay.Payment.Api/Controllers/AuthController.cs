using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPay.Payment.Common;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IHttpContextAccessor _accessor;
        private readonly IAuthService _authService;


        public AuthController(IHttpContextAccessor accessor, IAuthService authService)
        {
            _accessor = accessor;
            _authService = authService;
        }

        [HttpGet("echo")]
        public Task<string> GetRemoteIp()
        {
            return Task.FromResult(_accessor.HttpContext.Connection.RemoteIpAddress.ToString());
        }


        [HttpPost("test-auth")]
        public async Task<ActionResult<AuthenticationResult>> PostTestAuth(AuthenticationParam entity)
        {
            if (string.IsNullOrWhiteSpace(entity.IpAddress))
            {
                entity.IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            var result = await _authService.TestAuthentication(entity);

            return result;
        }

        [HttpPost("test-login")]
        public async Task<ActionResult<bool>> PostTestLogin(Common.DataWebService.CommonData entity)
        {
            var result = await _authService.Login(entity);

            return result;
        }

    }
}
