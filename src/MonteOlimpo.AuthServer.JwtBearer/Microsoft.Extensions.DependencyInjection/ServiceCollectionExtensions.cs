using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonteOlimpo.Authentication.JwtBearer;
using MonteOlimpo.Base.Extensions.Configuration;
using MonteOlimpo.Identity.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserPrincipalBuilder(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));


            services.AddScoped(typeof(IUserPrincipalBuilder), typeof(UserPrincipalBuilder));

            return services;
        }
     
        public static IServiceCollection AddJwtAuthenticationProvider(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddScoped(typeof(IUserPrincipalTokenizer), typeof(UserPrincipalTokenizer));
            services.AddTransient<SecurityTokenHandler, JwtSecurityTokenHandler>();

            return services;
        }

    }
}