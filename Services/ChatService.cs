using AutoMapper;
using Connectify.DTOs;
using Connectify.Interfaces;
using Connectify.Models;

namespace Connectify.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;

        private readonly IMapper _mapper;

        public ChatService(
            IChatRepository repository,
            IMapper mapper)
        {
            _repository = repository;

           _mapper = mapper;
        }

        public async Task<List<ChatMessageDto>> GetMessagesAsync()
        {
            var messages =
                await _repository.GetAllMessagesAsync();

            return _mapper.Map<List<ChatMessageDto>>(messages);
            
        }

        public async Task<ChatMessageDto> SaveMessageAsync(
            string userId,
            string userName,
            string message)
        {
            var chatMessage = new ChatMessage
            {
                UserId = userId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            await _repository.AddMessageAsync(chatMessage);

            await _repository.SaveChangesAsync();

            return new ChatMessageDto
            {
                UserId = userId,
                UserName = userName,
                Message = message,
                SentAt = chatMessage.SentAt
            };
        }
    }
}