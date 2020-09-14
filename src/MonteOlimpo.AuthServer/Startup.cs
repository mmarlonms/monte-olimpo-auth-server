using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonteOlimpo.AuthServer.Identity;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore;
using MonteOlimpo.Base.ApiBoot;
using MonteOlimpo.Base.Authentication;
using MonteOlimpo.Base.Core.CrossCutting;

namespace MonteOlimpo.AuthServer
{
    public class Startup : MonteOlimpoBootStrap
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.RegisterMonteOlimpoDataBase<ApplicationDbContext>(Configuration);
            services.AddAuthIdentity(Configuration.GetSection("IdentityConfiguration").Get<IdentityConfiguration>(), AddIdentityOptions);
            services.AddJWTAuthentication(Configuration.GetSection("TokenConfigurations").Get<TokenConfigurations>());
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            base.Configure(app, env);
            app.RunAuthIdentityMigrations();
            app.RunAuthIdentityInitializer();
        }

        protected virtual void AddIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.SignIn.RequireConfirmedEmail = true;
        }
    }
}
