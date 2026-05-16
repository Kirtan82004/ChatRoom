using Connectify.Models;
using Connectify.Data;
using Microsoft.AspNetCore.SignalR;


namespace Connectify.Hubs
{
    
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
                return;

            var userName = Context.User?.Identity?.Name ?? "Guest";

            var chatMessage = new ChatMessage
            {
                Message = message,
                UserId = "demo", // TEMP SAFE
                SentAt = DateTime.UtcNow,
            };

            try
            {
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB ERROR: " + ex.Message);
            }

            await Clients.All.SendAsync(
                "ReceiveMessage",
                userName,
                message,
                chatMessage.SentAt.ToString("yyyy-MM-dd HH:mm:ss")
            );
        }
    }
}
