using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ServiceMethodHandler> _logger;

        public ServiceMethodHandler(IAuthService authService, ILogger<ServiceMethodHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ServiceMethodRequirement requirement)
        {
            _logger.LogInformation("Authorization: Check Claims");

            // If the user does not have a name then we can not validate this requirement
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return;
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"Authorization: Is User {userId} authorized");

            var (permissionName, action) = requirement.Permission.GetPermissionInfo();

            if (await _authService.IsAuthorized(userId, permissionName, action))
            {

                _logger.LogInformation($"Authorization: User {userId} Authorized!");
                context.Succeed(requirement);
            }


            _logger.LogInformation($"Authorization: User {userId} completed");
        }
    }
}
