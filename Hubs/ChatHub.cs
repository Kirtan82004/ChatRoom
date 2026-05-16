using Connectify.Models;
using Connectify.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Connectify.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var userName = Context.User?.Identity?.Name;
            
            var userId = Context.UserIdentifier;

            var chatMessage = new ChatMessage
            {
                Message = message,
                UserId = userId,
                SentAt = DateTime.UtcNow,
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync(
                "ReceiveMessage",
                userName,
                message,
                chatMessage.SentAt.ToString("yyyy-MM-dd HH:mm:ss")
                );
        }
    }
}
