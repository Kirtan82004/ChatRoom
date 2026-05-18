using Connectify.Data;
using Connectify.Interfaces;
using Connectify.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectify.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetAllMessagesAsync()
        {
            return await _context.ChatMessages
                .Include(x => x.User)
                .OrderBy(x => x.SentAt)
                .ToListAsync();
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}