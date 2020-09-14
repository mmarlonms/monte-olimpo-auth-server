using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore;
using MonteOlimpo.Base.Core.Domain.UnitOfWork;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder RunAuthIdentityMigrations(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<ApplicationDbContext>();

                var identityConfiguration = serviceScope.ServiceProvider.GetService<IdentityConfiguration>();
                if (identityConfiguration.EnableMigrations)
                {
                    logger.LogInformation("Applying migrations for Identity...");

                    using (var context = serviceScope.ServiceProvider.GetService<IUnitOfWork>().Context)
                        context.Database.Migrate();


                    logger.LogInformation("Identity migrations applied.");
                }
                else
                    logger.LogInformation("Identity migrations disabled.");
            }

            return applicationBuilder;
        }

        public static IApplicationBuilder RunAuthIdentityInitializer(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var identityConfiguration = serviceScope.ServiceProvider.GetService<IdentityConfiguration>();
                if (identityConfiguration.EnableMigrations)
                {
                    serviceScope.ServiceProvider.GetService<IdentityInitializer>().InitializeRoles();
                    serviceScope.ServiceProvider.GetService<IdentityInitializer>().InitializeUsersAsync().Wait();
                }
            }

            return applicationBuilder;
        }
    }
}