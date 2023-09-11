using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
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
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
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
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "http" },
                                Scheme = "basic",
                                Name = "basic",
                                In = ParameterLocation.Header
                            },
                            new string[] { }
                        }
                    }
                };
            }
        }
    }
}
