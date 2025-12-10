using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureChat.Web.Models;

namespace SecureChat.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(u => u.DisplayName)
                .HasMaxLength(64);

            builder.Entity<ApplicationUser>()
                .Property(u => u.AvatarUrl)
                .HasMaxLength(256);
        }
    }
}
