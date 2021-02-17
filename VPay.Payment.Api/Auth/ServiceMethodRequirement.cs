using System;
using Microsoft.AspNetCore.Authorization;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    /// <inheritdoc />
    public class ServiceMethodRequirement : IAuthorizationRequirement
    {
        public ServicePermission Permission { get; }

        public ServiceMethodRequirement(ServicePermission permission)
        {
            Permission = permission;
        }
    }
}
