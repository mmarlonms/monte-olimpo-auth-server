using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserEntity, IdentityRoleEntity, Guid, IdentityUserClaimEntity, IdentityUserRole, IdentityUserLogin<Guid>, IdentityRoleClaimEntity, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging()
                 .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRoleEntity>(portal =>
            {
                portal.Ignore(u => u.Claims);
            });

            builder.Entity<IdentityUserEntity>(user =>
            {
                user.Ignore(u => u.Claims);
                user.Ignore(u => u.Roles);

            });

            builder.Entity<IdentityUserRole>(userRole =>
            {
                userRole.HasOne(ur => ur.IdentityRole)
                    .WithMany(r => r.IdentityUserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.IdentityUser)
                    .WithMany(u => u.IdentityUserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<IdentityRoleClaimEntity>(roleClaim =>
            {
                roleClaim.HasOne(uc => uc.Role)
                    .WithMany(u => u.IdentityRoleClaims)
                    .HasForeignKey(uc => uc.RoleId)
                    .IsRequired();
            });

            builder.Entity<IdentityUserClaimEntity>(userClaim =>
            {
                userClaim.HasOne(uc => uc.User)
                    .WithMany(u => u.IdentityClaims)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
            });
        }
    }
}