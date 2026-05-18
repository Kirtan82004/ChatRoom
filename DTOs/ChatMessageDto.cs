namespace Connectify.DTOs
{
    public class ChatMessageDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; }
    }
}
