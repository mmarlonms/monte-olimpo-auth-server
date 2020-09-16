using System;
using Microsoft.AspNetCore.Identity;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore.Stores;
using MonteOlimpo.Identity.EntityFrameworkCore.Managers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthIdentity(this IServiceCollection services, IdentityConfiguration identityConfiguration, Action<IdentityOptions> identityOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(identityConfiguration ?? throw new ArgumentNullException(nameof(identityConfiguration)));

            services.AddIdentity<IdentityUserEntity, IdentityRoleEntity>(identityOptions)
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders()
                 .AddUserStore<UserStores>()
                 .AddUserManager<MonteOlimpoUserManager>();

            services.AddScoped<IdentityInitializer>();

            return services;
        }
    }
}