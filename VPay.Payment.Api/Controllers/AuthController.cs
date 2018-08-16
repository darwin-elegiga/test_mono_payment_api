using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPay.Payment.Common;
using VPay.Payment.Common.Db2;
using VPay.Payment.Common.Models;

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
        [Authorize]
        public Task<string> GetRemoteIp()
        {
            var ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress;
            ipAddress = ipAddress.MapToIPv4();
            return Task.FromResult(ipAddress.ToString());
        }


        [HttpPost("test-auth")]
        public async Task<ActionResult<AuthenticationResult>> PostTestAuth(AuthenticationParam entity)
        {
            if (string.IsNullOrWhiteSpace(entity.IpAddress))
            {
                var ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress;
                ipAddress = ipAddress.MapToIPv4();
                entity.IpAddress = ipAddress.ToString();
            }

            var result = await _authService.TestAuthentication(entity);

            return result;
        }

        [HttpPost("test-login")]
        public async Task<ActionResult<UserSessionInfo>> PostTestLogin(AuthenticationParam entity)
        {
            var result = await _authService.Login(entity);

            return result;
        }

    }
}
