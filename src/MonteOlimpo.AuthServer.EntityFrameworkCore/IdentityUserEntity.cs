using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MonteOlimpo.Identity.Abstractions;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityUserEntity : IdentityUser<Guid>, IUserPrincipal
    {
        public virtual string ApplicationStamp { get; set; }
        public string FullName { get; set; }
        public virtual DateTimeOffset? LockoutEndDateUtc { get; set; }
        public virtual DateTime LastActivityDate { get; set; } = DateTime.Now;
        public virtual DateTime Date { get; set; } = DateTime.Now;
        public virtual DateTime LastPasswordChangedDate { get; set; } = DateTime.Now;
        public virtual ICollection<IdentityUserRole> IdentityUserRoles { get; set; }
        public virtual ICollection<IdentityUserClaimEntity> IdentityClaims { get; set; }
        public virtual ICollection<Claim> Claims
        {
            get
            {
                return IdentityClaims?.Select(ic => ic.ToClaim()).ToList();
            }
            set
            {
                IdentityClaims = value.Select(c => new IdentityUserClaimEntity()
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();
            }
        }
        public virtual ICollection<IRolePrincipal> Roles
        {
            get
            {
                return IdentityUserRoles?.Select(_ => _.IdentityRole)?.ToList<IRolePrincipal>();
            }
        }
    }
}
