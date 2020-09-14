using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MonteOlimpo.Identity.Abstractions;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityRoleEntity : IdentityRole<Guid>, IRolePrincipal
    {
        public virtual ICollection<IdentityRoleClaimEntity> IdentityRoleClaims { get; set; }

        public virtual ICollection<IdentityUserRole> IdentityUserRoles { get; set; }

        public virtual ICollection<Claim> Claims
        {
            get
            {
                return IdentityRoleClaims?.Select(ic => ic.ToClaim()).ToList();
            }
            set
            {
                IdentityRoleClaims = value.Select(c => new IdentityRoleClaimEntity()
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();
            }
        }
    }
}
