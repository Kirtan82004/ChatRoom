using Connectify.Models;
using Connectify.Data;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Connectify.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var userName = Context.User?.Identity?.Name ?? "Guest";

            if (string.IsNullOrEmpty(userId))
                return;

            var chatMessage = new ChatMessage
            {
                Message = message.Trim(),
                UserId = userId,
                SentAt = DateTime.UtcNow
            };

            try
            {
                _context.ChatMessages.Add(chatMessage);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB ERROR: " + ex.Message);

                return;
            }

            await Clients.All.SendAsync(
                "ReceiveMessage",
                userName,
                chatMessage.Message,
                chatMessage.SentAt
                    .ToLocalTime()
                    .ToString("hh:mm tt")
            );
        }
    }
}