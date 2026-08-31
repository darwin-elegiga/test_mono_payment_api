using System.Linq;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace VPay.Payment.Api
{
    public static class ActionDescriptorExtensions
    {
        public static ApiVersionModel GetApiVersion(this ActionDescriptor actionDescriptor)
        {
            // Asp.Versioning stores ApiVersionMetadata in EndpointMetadata instead of the
            // ApiVersionModel entry the legacy versioning package kept in Properties.
            return actionDescriptor?.EndpointMetadata?
                .OfType<ApiVersionMetadata>()
                .Select(m => m.Map(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit))
                .FirstOrDefault();
        }
    }
}
