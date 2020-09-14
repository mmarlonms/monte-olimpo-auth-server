using Microsoft.AspNetCore.Identity;
using System;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityUserClaimEntity : IdentityUserClaim<Guid>
    {
        public virtual IdentityUserEntity User { get; set; }
    }
}