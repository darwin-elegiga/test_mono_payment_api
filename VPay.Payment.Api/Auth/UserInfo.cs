using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    public class UserInfo : IUserInfo
    {
        private readonly IHttpContextAccessor _accessor;

        public UserInfo(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserName => _accessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        public string Token => _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string Source => _accessor.HttpContext.User.FindFirstValue(ClaimTypes.System);

        public bool IsAuthenticated => _accessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
