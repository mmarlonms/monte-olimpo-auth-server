using Microsoft.AspNetCore.Identity;
using System;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityUserRole : IdentityUserRole<Guid>
    {
        public virtual IdentityRoleEntity IdentityRole { get; set; }

        public virtual IdentityUserEntity IdentityUser { get; set; }
    }
}