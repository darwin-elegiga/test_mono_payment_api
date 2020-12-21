using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly string _securitySchemaName;

        public BasicAuthFilter(string securitySchemaName = "jwt")
        {
            _securitySchemaName = securitySchemaName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (GetControllerAndActionAttributes<AllowAnonymousAttribute>(context).Any())
            {
                return;
            }

            var actionAttributes = GetControllerAndActionAttributes<AuthorizeAttribute>(context);

            if (!actionAttributes.Any())
            {
                return;
            }

            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }

            if (!operation.Responses.ContainsKey("403"))
            {
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }

            var policies = actionAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Policy))
                    .Select(a => a.Policy);

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = _securitySchemaName } }, policies.ToList() }
            });
        }

        public static IEnumerable<T> GetControllerAndActionAttributes<T>(OperationFilterContext context) where T : Attribute
        {
            var controllerAttributes = context.MethodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
            var actionAttributes = context.MethodInfo.GetCustomAttributes<T>();

            var result = new List<T>(controllerAttributes);
            result.AddRange(actionAttributes);
            return result;
        }


    }
}
