using Microsoft.AspNetCore.Identity;

namespace SecureChat.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }   // e.g., "Luna"
        public string? AvatarUrl { get; set; }     // optional: Gravatar or custom
    }
}
