using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MonteOlimpo.Identity.Abstractions;

namespace MonteOlimpo.Authentication.JwtBearer
{
    public class UserPrincipalTokenizer : IUserPrincipalTokenizer
    {
        private readonly SecurityTokenHandler tokenHandler;
        private readonly JwtConfiguration jwtConfiguration;

        public UserPrincipalTokenizer(
            SecurityTokenHandler tokenHandler,
            JwtConfiguration jwtConfiguration
            )
        {
            this.tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
            this.jwtConfiguration = jwtConfiguration ?? throw new ArgumentNullException(nameof(jwtConfiguration));
        }

        public GenerateTokenResult GenerateToken(IUserPrincipal userPrincipal)
        {
            //Caso o usuário só tenha só uma Role é gerado as permissões da quela role
            if (userPrincipal.Roles != null && userPrincipal.Roles.Count == 1)
            {
                var roleName = userPrincipal.Roles.FirstOrDefault().Name;
                return GenerateToken(userPrincipal, roleName);
            }

            return GenerateToken(GenerateClaimsIdentity(userPrincipal));
        }

        public GenerateTokenResult GenerateToken(IUserPrincipal userPrincipal, string roleName)
        {
            var identity = GenerateClaimsIdentity(userPrincipal);

            var role = userPrincipal.Roles.Where(_ => _.Name == roleName).FirstOrDefault();

            if (role == null)
                return null;

            foreach (var roleClaims in role.Claims)
                identity.AddClaim(new Claim(roleClaims.Type, roleClaims.Value));

            identity.AddClaim(new Claim(ClaimNames.UserRole, role.Name));

            //Monta as outras permissões
            if (userPrincipal.Claims != null && userPrincipal.Claims.Any())
                foreach (var claim in userPrincipal.Claims.Where(c => c.Type != ClaimNames.ClientId && c.Type != ClaimNames.CnetPermission))
                    identity.AddClaim(new Claim(ClaimNames.Permission, claim.Value));

            return GenerateToken(identity);
        }


        private ClaimsIdentity GenerateClaimsIdentity(IUserPrincipal userPrincipal)
        {
            var identity = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Sub, userPrincipal.UserName)
                }
            );

            if (!string.IsNullOrWhiteSpace(userPrincipal.FullName))
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName, userPrincipal.Email));

            if (!string.IsNullOrWhiteSpace(userPrincipal.Email))
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.Email));

            if (userPrincipal.Roles != null && userPrincipal.Roles.Any())
                foreach (var role in userPrincipal.Roles)
                    identity.AddClaim(new Claim(ClaimNames.Roles, role.Name));

            return identity;
        }

        private GenerateTokenResult GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var validIssuer = Guid.TryParse(jwtConfiguration.Issuer, out var guidIssuer)
             ? guidIssuer.ToString()
             : jwtConfiguration.Issuer;

            var validAudience = Guid.TryParse(jwtConfiguration.Audience, out var guidAudience)
                ? guidAudience.ToString()
                : jwtConfiguration.Audience;

            var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = validIssuer,
                Audience = validAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)), SecurityAlgorithms.HmacSha256),
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.ExpirationInMinutes)
            });

            var jwt = tokenHandler.WriteToken(securityToken);

            return new GenerateTokenResult(jwt, securityToken.ValidTo);
        }

        public bool ValidateToken(string authToken, bool validateTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(validateTime);

            tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            return true;
        }

        private TokenValidationParameters GetValidationParameters(bool validateTime)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = validateTime, // Because there is no expiration in the generated token
                ValidateAudience = true, // Because there is no audiance in the generated token
                ValidateIssuer = true,   // Because there is no issuer in the generated token
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)) // The same key as the one that generate the token
            };
        }
    }
}
