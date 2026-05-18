using Connectify.Models;

namespace Connectify.Interfaces
{
    public interface IChatRepository
    {
        Task<List<ChatMessage>> GetAllMessagesAsync();

        Task AddMessageAsync(ChatMessage message);

        Task SaveChangesAsync();
    }
}