using Connectify.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Connectify.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var messages =
                await _chatService.GetMessagesAsync();

            ViewBag.CurrentUserId = User
    .FindFirst(System.Security.Claims
    .ClaimTypes.NameIdentifier)?
    .Value;

            return View(messages);
        }
    }
}