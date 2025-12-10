namespace SecureChat.Web.Infrastructure
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.SignalR;

    public class NameIdUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
            => connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
