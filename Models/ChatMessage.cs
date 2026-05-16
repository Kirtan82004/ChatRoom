namespace Connectify.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
