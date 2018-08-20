using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VPay.Payment.Api.Auth
{
    /// <summary>
    /// This will allow the swagger to know which api methods need to use
    /// the VPay basic authentication 
    /// </summary>
    public class BasicAuthFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

            var classAuthorized = context.MethodInfo
                .DeclaringType
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>().Any();

            bool hasAuthorize;

            if (classAuthorized)
            {
                // If the class has the Authorize Attribute then need to test if the method overrides the authorize attribute to anonymous.
                hasAuthorize = !context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>().Any();
            }
            else
            {
                // If the class does not have the Authorize Attribute then test to see if the method itself has authorize attribute.
                hasAuthorize = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>().Any();
            }

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"VPay", new string[] { }}
                    }
                };
            }
        }

    }
}
