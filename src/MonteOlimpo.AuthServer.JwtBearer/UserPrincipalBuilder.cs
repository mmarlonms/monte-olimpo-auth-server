using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MonteOlimpo.Authentication.JwtBearer.Identity;
using MonteOlimpo.Identity.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MonteOlimpo.Authentication.JwtBearer
{
    public class UserPrincipalBuilder : IUserPrincipalBuilder
    {
        private UserPrincipal _userPrincipal;
        public IUserPrincipal UserPrincipal
        {
            get
            {
                if (_userPrincipal == null || !_userPrincipal.Claims.Any())
                    _userPrincipal = (UserPrincipal)ValidateClaimsAndBuildUserPrincipal();

                return _userPrincipal;
            }
        }

        protected IHttpContextAccessor HttpContextAccessor { get; }

        public UserPrincipalBuilder(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetAccessToken()
        {
            return UserPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimNames.AccessToken && !string.IsNullOrWhiteSpace(c.Value))?.Value;
        }

        public Guid GetCurrentClientId()
        {
            return new Guid(UserPrincipal?.Claims.Single(c => c.Type == ClaimNames.ClientId).Value);
        }

        private IUserPrincipal ValidateClaimsAndBuildUserPrincipal()
        {
            var userPrincipal = new UserPrincipal()
            {
                Claims = new List<Claim>(),
            };

            if (!(HttpContextAccessor.HttpContext?.User?.Identity is ClaimsIdentity claimsIdentity) || !claimsIdentity.Claims.Any())
                return userPrincipal;


            foreach (var claim in claimsIdentity.Claims)
            {
                var claimShortTypeName = claim.Properties.SingleOrDefault(p => p.Key.Contains("ShortTypeName")).Value;
                if (string.IsNullOrWhiteSpace(claimShortTypeName))
                    claimShortTypeName = claim.Type;

                switch (claimShortTypeName)
                {
                    case (JwtRegisteredClaimNames.Sub):
                        userPrincipal.UserName = claim.Value;
                        break;
                    case (JwtRegisteredClaimNames.Aud):
                        userPrincipal.Claims.Add(new Claim(ClaimNames.ClientId, claim.Value));
                        break;
                    case (JwtRegisteredClaimNames.GivenName):
                        userPrincipal.FullName = claim.Value;
                        break;
                    case (JwtRegisteredClaimNames.Email):
                        userPrincipal.Email = claim.Value;
                        break;
                    case (ClaimNames.PortalCode):
                        userPrincipal.Portal = Convert.ToInt32(claim.Value);
                        break;
                    case (ClaimTypes.Role):
                        new Claim(ClaimTypes.Role, claim.Value);
                        break;
                    default:
                        userPrincipal.Claims.Add(new Claim(claim.Type, claim.Value));
                        break;
                }
            }

            if (!userPrincipal.Claims.Any(c => c.Type == ClaimNames.AccessToken && !string.IsNullOrWhiteSpace(c.Value)))
            {
                var accessToken = HttpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(accessToken))
                    userPrincipal.Claims.Add(new Claim(ClaimNames.AccessToken, accessToken));
            }

            return userPrincipal;
        }

    }
}