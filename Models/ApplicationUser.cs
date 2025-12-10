using AspNetCore.Identity.MongoDbCore.Models;

namespace SecureChat.Web.Models
{
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string? DisplayName { get; set; }   // e.g., "Luna"
        public string? AvatarUrl { get; set; }     // optional: Gravatar or custom
    }
}
