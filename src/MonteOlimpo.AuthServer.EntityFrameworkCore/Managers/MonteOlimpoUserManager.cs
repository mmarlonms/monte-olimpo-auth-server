using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonteOlimpo.AuthServer.Identity.EntityFrameworkCore;

namespace MonteOlimpo.Identity.EntityFrameworkCore.Managers
{
    public class MonteOlimpoUserManager : UserManager<IdentityUserEntity>
    {
        public MonteOlimpoUserManager(IUserStore<IdentityUserEntity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<IdentityUserEntity> passwordHasher, IEnumerable<IUserValidator<IdentityUserEntity>> userValidators, IEnumerable<IPasswordValidator<IdentityUserEntity>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUserEntity>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}