using System;
using MonteOlimpo.Identity.Abstractions;

namespace MonteOlimpo.AuthServer.Dto
{
    public class AuthPostResult
    {
        public string AccessToken { get; set; }
        public DateTime? Expiration { get; set; }

        public AuthPostResult()
        {
        }

        public AuthPostResult(GenerateTokenResult generateTokenResult)
        {
            AccessToken = generateTokenResult.AccessToken;
            Expiration = generateTokenResult.Expiration;
        }
    }
}