using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityInitializer
    {
        private readonly UserManager<IdentityUserEntity> userManager;
        private readonly RoleManager<IdentityRoleEntity> roleManager;

        private readonly ILogger logger;

        public IdentityInitializer(
            UserManager<IdentityUserEntity> userManager,
            RoleManager<IdentityRoleEntity> roleManager,
            ILoggerFactory loggerFactory)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

            logger = loggerFactory?.CreateLogger<IdentityInitializer>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }


        public void InitializeRoles()
        {
            var roleAdmin = new IdentityRoleEntity() { Name = "admin", NormalizedName = "Admin" };
            var roleUser = new IdentityRoleEntity() { Name = "user", NormalizedName = "User" };

            CreateRoleAsync(roleAdmin);
            CreateRoleAsync(roleUser);
        }

        public async Task InitializeUsersAsync()
        {
            await CreateUserAsync(new IdentityUserEntity()
            {
                UserName = "admin",
                NormalizedUserName = "Admin",
                EmailConfirmed = true,

            }, "admin");

            await CreateUserAsync(new IdentityUserEntity()
            {
                UserName = "user",
                NormalizedUserName = "User",
                EmailConfirmed = true,

            }, "user");
        }

        private void CreateRoleAsync(IdentityRoleEntity role)
        {
            try
            {
                var resultado = roleManager.CreateAsync(role).Result;

                if (resultado.Succeeded)
                    logger.LogInformation($"Role Admin created.");
                else
                    logger.LogWarning($"An error occurred while creating the role Admin.");

                logger.LogInformation("Database filled.");
            }
            catch (Exception e)
            {
                logger.LogError(e, "An internal error occurred while trying to load roles.");
            }
        }

        private async Task CreateUserAsync(IdentityUserEntity user, string additionalRoles = null)
        {
            var userEntity = await userManager.FindByNameAsync(user.UserName);
            if (userEntity == null)
            {
                var creationResult = await userManager.CreateAsync(user, user.UserName.ToLower());
                if (creationResult.Succeeded)
                {
                    logger.LogInformation($"User '{user}' created.");

                    userEntity = await userManager.FindByNameAsync(user.UserName);
                }
                else
                {
                    logger.LogWarning($"An error occurred while creating the user '{user}'.");
                    return;
                }
            }

            if (additionalRoles != null)
                await userManager.AddToRoleAsync(userEntity, additionalRoles);

            logger.LogInformation($"Default roles and claims set to user '{user}'.");
        }

    }
}
