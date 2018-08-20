using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    public static class VPayAuthenticationExtensions
    {
        public static AuthenticationBuilder AddVPay<TAuthService>(this AuthenticationBuilder builder)
            where TAuthService : class, IAuthService
        {
            return AddVPay<TAuthService>(builder, VPayAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddVPay<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IAuthService
        {
            return AddVPay<TAuthService>(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddVPay<TAuthService>(this AuthenticationBuilder builder, Action<VPayAuthenticationOptions> configureOptions)
            where TAuthService : class, IAuthService
        {
            return AddVPay<TAuthService>(builder, VPayAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddVPay<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<VPayAuthenticationOptions> configureOptions)
            where TAuthService : class, IAuthService
        {
            builder.Services.AddTransient<IAuthService, TAuthService>();

            return builder.AddScheme<VPayAuthenticationOptions, VPayAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
