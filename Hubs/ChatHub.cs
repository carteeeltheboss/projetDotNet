namespace SecureChat.Web.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using SecureChat.Web.Models;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _users;
        public ChatHub(UserManager<ApplicationUser> users) => _users = users;

        private async Task<string> LabelFor(string userId)
        {
            var u = await _users.FindByIdAsync(userId);
            return !string.IsNullOrWhiteSpace(u?.DisplayName) ? u!.DisplayName! : (u?.Email ?? userId);
        }

        public async Task SendDirectByEmail(string toEmail, string message)
        {
            var fromId = Context.UserIdentifier!;
            var toUser = await _users.FindByEmailAsync(toEmail)
                ?? throw new HubException("User not found.");
            var fromLabel = await LabelFor(fromId);
            await Clients.User(toUser.Id)
                .SendAsync("ReceiveDirect", fromLabel, message, DateTime.UtcNow);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            var who = await LabelFor(Context.UserIdentifier!);
            await Clients.Group(roomId).SendAsync("System", $"{who} joined {roomId}");
        }

        public async Task SendToRoom(string roomId, string message)
        {
            var from = await LabelFor(Context.UserIdentifier!);
            await Clients.Group(roomId).SendAsync("ReceiveRoom", roomId, from, message, DateTime.UtcNow);
        }
    }
}
