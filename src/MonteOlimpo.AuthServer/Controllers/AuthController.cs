using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonteOlimpo.AuthServer.Dto;
using MonteOlimpo.Base.ApiBoot;
using MonteOlimpo.Base.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MonteOlimpo.AuthServer.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly TokenConfigurations tokenConfigurations;

        public AuthController(TokenConfigurations tokenConfigurations)
        {
            this.tokenConfigurations = tokenConfigurations;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> LoginAsync(
           [FromBody]AuthDto authDto,
           [FromServices] UserManager<IdentityUser> userManager)
        {

            IdentityUser identityUser = new IdentityUser();

            bool credenciaisValidas = false;

            if (authDto != null && !String.IsNullOrWhiteSpace(authDto.UserName) && !String.IsNullOrWhiteSpace(authDto.UserPassword))
            {
                identityUser = await userManager.FindByNameAsync(authDto.UserName);
                credenciaisValidas = await userManager.CheckPasswordAsync(identityUser, authDto.UserPassword);
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(identityUser.Id.ToString(), "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName,identityUser.Id.ToString())
                    }
                );


                var roles = await userManager.GetRolesAsync(identityUser);

                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                identity.AddClaim(new Claim("user_id", identityUser.Id.ToString()));

                identity.AddClaim(new Claim("username", identityUser.UserName));
                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret)), SecurityAlgorithms.HmacSha256),
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });


                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = true, // Because there is no audiance in the generated token
                ValidateIssuer = true,   // Because there is no issuer in the generated token
                ValidIssuer = this.tokenConfigurations.Issuer,
                ValidAudience = this.tokenConfigurations.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.tokenConfigurations.Secret)) // The same key as the one that generate the token
            };
        }

        private bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            return true;
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public object RefreshToken(
           string token)
        {

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            if (ValidateToken(token))
            {

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}
