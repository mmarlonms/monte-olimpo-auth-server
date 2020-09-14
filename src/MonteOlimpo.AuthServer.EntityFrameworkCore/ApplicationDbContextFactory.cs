using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=localhost;Database=authserver;User ID=sa;Password=123@MA456",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new ApplicationDbContext(builder.Options);
        }
    }
}