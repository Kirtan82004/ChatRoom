using Microsoft.AspNetCore.Identity;

namespace Connectify.Models
{
    public class User: IdentityUser
    {
        public string? FullName { get; set; } = string.Empty;
    }
}
