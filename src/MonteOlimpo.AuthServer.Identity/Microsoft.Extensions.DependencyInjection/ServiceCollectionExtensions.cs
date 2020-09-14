
using Microsoft.AspNetCore.Identity;
using MonteOlimpo.AuthServer.Identity;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthIdentity(this IServiceCollection services, IdentityConfiguration identityConfiguration, Action<IdentityOptions> identityOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(identityConfiguration ?? throw new ArgumentNullException(nameof(identityConfiguration)));

            services.AddIdentity<IdentityUser, IdentityRole>(identityOptions)
               .AddEntityFrameworkStores<IdentityContext>()
               .AddDefaultTokenProviders();

            services.AddScoped<IdentityInitializer>();

            return services;
        }
    }
}