namespace Connectify.Web.ViewModels
{
    public class ChatMessageViewModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.MinValue;
    }
}