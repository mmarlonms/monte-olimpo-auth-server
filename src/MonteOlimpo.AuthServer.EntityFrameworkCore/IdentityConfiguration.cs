using System.ComponentModel.DataAnnotations;

namespace MonteOlimpo.AuthServer.Identity.EntityFrameworkCore
{
    public class IdentityConfiguration
    {
        [Required]
        public bool EnableMigrations { get; set; } = false;
        public bool EnableInitializer { get; set; } = false;
    }
}