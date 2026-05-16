using Microsoft.AspNetCore.Authorization;
using Connectify.Web.ViewModels;
using Connectify.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connectify.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Messages = await _context.ChatMessages
                .Include(u => u.User)
                .OrderBy(o => o.SentAt)
                .Select(s => new ChatMessageViewModel
                {
                    UserName = s.User.FullName,
                    Message = s.Message,
                    SentAt = s.SentAt
                }).ToListAsync();

            return View(Messages);
        }
    }
}
