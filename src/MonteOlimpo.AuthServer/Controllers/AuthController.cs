using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonteOlimpo.Authentication.JwtBearer;
using MonteOlimpo.AuthServer.Dto;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore;
using MonteOlimpo.Base.ApiBoot;
using MonteOlimpo.Identity.Abstractions;

namespace MonteOlimpo.AuthServer.Controllers
{
    public class AuthController : ApiBaseController
    {
        public IUserPrincipalTokenizer UserPrincipalTokenizer { get; }
        public JwtConfiguration JwtConfiguration { get; }
        public UserManager<IdentityUserEntity> AppUserManager { get; }
        public ILogger<AuthController> Logger { get; }

        public AuthController(
            IUserPrincipalTokenizer userPrincipalTokenizer,
            JwtConfiguration jwtConfiguration,
            UserManager<IdentityUserEntity> appUserManager,
            ILogger<AuthController> logger)
        {
            UserPrincipalTokenizer = userPrincipalTokenizer;
            JwtConfiguration = jwtConfiguration;
            AppUserManager = appUserManager;
            Logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> LoginAsync([FromBody]AuthDto authDto)
        {
            IdentityUserEntity identityUser = null;

            bool credenciaisValidas = false;

            if (authDto != null && !String.IsNullOrWhiteSpace(authDto.UserName) && !String.IsNullOrWhiteSpace(authDto.UserPassword))
            {
                var normalizedUserName = authDto.UserName.ToUpper().Trim();
                identityUser = await AppUserManager.Users
                  .Include(u => u.IdentityUserRoles)
                      .ThenInclude(ur => ur.IdentityRole)
                        .ThenInclude(ur => ur.IdentityRoleClaims)
                    .Include(u => u.IdentityUserRoles)
                          .ThenInclude(ur => ur.IdentityRole)
                  .Include(u => u.IdentityClaims)

                  .SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);

                if (identityUser != null)
                    credenciaisValidas = await AppUserManager.CheckPasswordAsync(identityUser, authDto.UserPassword);

            }

            if (credenciaisValidas)
            {
                Logger.LogInformation("Login realizado as {UserName}", DateTime.Now.ToString());
                return Ok(new AuthPostResult(UserPrincipalTokenizer.GenerateToken(identityUser)));
            }

            if (!credenciaisValidas)
                return SemPermissao(authDto);

            return FalhaAoAntenticar(authDto);
        }

        [AllowAnonymous]
        [HttpPost("login/token")]
        public async Task<object> LoginWithTokenAsync([FromBody]AuthTokenDto authTokenDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(authTokenDto.Token) as JwtSecurityToken;

            if (!UserPrincipalTokenizer.ValidateToken(authTokenDto.Token, false))
                FalhaAoAntenticar(null);

            var userName = tokenS.Claims.Where(_ => _.Type == "email").FirstOrDefault().Value;
            var portal = Convert.ToInt32(tokenS.Claims.Where(_ => _.Type == ClaimNames.PortalCode).FirstOrDefault().Value);

            IdentityUserEntity identityUser = null;

            var normalizedUserName = userName.ToUpper().Trim();
            identityUser = await AppUserManager.Users
              .Include(u => u.IdentityUserRoles)
                  .ThenInclude(ur => ur.IdentityRole)
                   .ThenInclude(ur => ur.IdentityRoleClaims)
              .Include(u => u.IdentityClaims)
              .SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);

            var token = UserPrincipalTokenizer.GenerateToken(identityUser, authTokenDto.Role);
            if (token != null)
                return Ok(new AuthPostResult(token));

            return SemPermissao(null);
        }

        private object FalhaAoAntenticar(AuthDto authDto)
        {
            Logger.LogWarning("Falha ao autenticar o usuário {UserName}.", authDto?.UserName);
            return new
            {
                authenticated = false,
                message = "Falha ao autenticar"
            };
        }

        private object SemPermissao(AuthDto authDto)
        {
            Logger.LogWarning("Acesso indevido pelo usuário {UserName}.", authDto?.UserName);
            return new
            {
                authenticated = false,
                message = "Sem permissão"
            };
        }
    }
}