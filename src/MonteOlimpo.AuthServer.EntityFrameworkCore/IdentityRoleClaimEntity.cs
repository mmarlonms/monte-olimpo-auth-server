using Microsoft.AspNetCore.Identity;
using System;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityRoleClaimEntity : IdentityRoleClaim<Guid>
    {
        public virtual IdentityRoleEntity Role { get; set; }
    }
}