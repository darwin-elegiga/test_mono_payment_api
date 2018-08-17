using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    public static class VPayAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasic<TAuthService>(this AuthenticationBuilder builder)
            where TAuthService : class, IAuthService
        {
            return AddBasic<TAuthService>(builder, VPayAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddBasic<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IAuthService
        {
            return AddBasic<TAuthService>(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddBasic<TAuthService>(this AuthenticationBuilder builder, Action<VPayAuthenticationOptions> configureOptions)
            where TAuthService : class, IAuthService
        {
            return AddBasic<TAuthService>(builder, VPayAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddBasic<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<VPayAuthenticationOptions> configureOptions)
            where TAuthService : class, IAuthService
        {
            builder.Services.AddTransient<IAuthService, TAuthService>();

            return builder.AddScheme<VPayAuthenticationOptions, VPayAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
