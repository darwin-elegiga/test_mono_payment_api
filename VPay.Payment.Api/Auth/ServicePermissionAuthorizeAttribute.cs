using System;
using Microsoft.AspNetCore.Authorization;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    /// <inheritdoc />
    public class ServicePermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "ServicePermission_";

        public ServicePermissionAuthorizeAttribute(ServicePermission permission)
        {
            Permission = permission;
        }

        public ServicePermission Permission
        {
            get => Enum.TryParse<ServicePermission>(Policy.Substring(PolicyPrefix.Length), out var permission) ? permission : default(ServicePermission);
            set => Policy = $"{PolicyPrefix}{value.ToString()}";
        }

    }
}
