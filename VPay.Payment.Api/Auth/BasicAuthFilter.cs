using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VPay.Payment.Api.Auth
{
    public class BasicAuthFilter : IOperationFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirements = new Dictionary<string, IEnumerable<string>>()
            {
                {"VPay", new string[] { }}
            };

            swaggerDoc.Security = new[] {securityRequirements};
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo
                                   .DeclaringType
                                   .GetCustomAttributes(true)
                                   .OfType<AuthorizeAttribute>().Any()
                               ||
                               context.MethodInfo
                                   .GetCustomAttributes(true)
                                   .OfType<AuthorizeAttribute>().Any();


            if (hasAuthorize)
            {

                var allowAnn = context.MethodInfo
                                   .DeclaringType
                                   .GetCustomAttributes(true)
                                   .OfType<AllowAnonymousAttribute>().Any()
                               ||
                               context.MethodInfo
                                   .GetCustomAttributes(true)
                                   .OfType<AllowAnonymousAttribute>().Any();
                if (allowAnn)
                {
                    return;
                }
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    {"VPay", new string[] { }}
                });
            }
        }

    }
}
