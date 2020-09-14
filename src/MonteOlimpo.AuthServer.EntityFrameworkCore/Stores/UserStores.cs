using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore.Stores
{
    internal class UserStores : UserStore<IdentityUserEntity, IdentityRoleEntity, ApplicationDbContext, Guid, IdentityUserClaimEntity, IdentityUserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaimEntity>

    {
        public UserStores(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}