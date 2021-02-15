using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    /// <inheritdoc />
    /// <summary>
    /// This class will handle authorizing any policy that has a ServiceMethodRequirement setup on it.
    /// </summary>
    public class ServiceMethodHandler : AuthorizationHandler<ServiceMethodRequirement>
    {

        private readonly IAuthService _authService;

        public ServiceMethodHandler(IAuthService authService)
        {
            _authService = authService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ServiceMethodRequirement requirement)
        {
            // If the user does not have a name then we can not validate this requirement
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return;
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            var (permissionName, action) = requirement.Permission.GetPermissionInfo();

            if (await _authService.IsAuthorized(userId, permissionName, action))
            {
                context.Succeed(requirement);
            }
        }
    }
}
