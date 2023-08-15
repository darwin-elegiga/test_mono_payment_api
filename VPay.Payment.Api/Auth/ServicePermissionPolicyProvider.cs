using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    /// <inheritdoc />
    public class ServicePermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string PolicyPrefix = "ServicePermission_";
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }


        public ServicePermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();


        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();


        /// <inheritdoc />
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase) &&
                Enum.TryParse<ServicePermission>(policyName.Substring(PolicyPrefix.Length), out var permission))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new ServiceMethodRequirement(permission));
                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
