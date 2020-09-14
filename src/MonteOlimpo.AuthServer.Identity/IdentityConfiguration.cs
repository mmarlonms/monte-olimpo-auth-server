using System.ComponentModel.DataAnnotations;

namespace MonteOlimpo.AuthServer.Identity
{
    public class IdentityConfiguration
    {
        public bool EnableMigrations { get; set; } = false;
        public bool EnableInitializer { get; set; } = false;
    }
}