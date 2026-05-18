using Connectify.DTOs;

namespace Connectify.Interfaces
{
    public interface IChatService
    {
        Task<List<ChatMessageDto>> GetMessagesAsync();

        Task<ChatMessageDto> SaveMessageAsync(
            string userId,
            string userName,
            string message);
    }
}