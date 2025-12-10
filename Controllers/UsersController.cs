namespace SecureChat.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SecureChat.Web.Models;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _users;
        public UsersController(UserManager<ApplicationUser> users) => _users = users;

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Ok(Array.Empty<object>());

            var q = _users.Users
                .Where(u =>
                    (u.Email != null && u.Email.ToLower().StartsWith(term.ToLower())) ||
                    (u.DisplayName != null && u.DisplayName.ToLower().StartsWith(term.ToLower())))
                .Select(u => new { u.Id, u.Email, u.DisplayName })
                .Take(10)
                .ToList();

            return Ok(q);
        }
    }
}
