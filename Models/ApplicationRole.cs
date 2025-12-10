using AspNetCore.Identity.MongoDbCore.Models;

namespace SecureChat.Web.Models
{
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
    }
}
