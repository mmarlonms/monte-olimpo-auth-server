using MonteOlimpo.Identity.Abstractions;
using System.Collections.Generic;
using System.Security.Claims;

namespace MonteOlimpo.Authentication.JwtBearer.Identity
{
    public class UserPrincipal : IUserPrincipal
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Portal { get; set; }
        public ICollection<Claim> Claims { get; set; }

        public ICollection<IRolePrincipal> Roles { get; }
    }
}