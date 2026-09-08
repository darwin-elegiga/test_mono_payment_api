using System.Linq;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace VPay.Payment.Api
{
    public static class ActionDescriptorExtensions
    {
        public static ApiVersionModel GetApiVersion(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor?.EndpointMetadata?
                .OfType<ApiVersionMetadata>()
                .Select(m => m.Map(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit))
                .FirstOrDefault();
        }
    }
}
