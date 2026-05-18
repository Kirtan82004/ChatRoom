using Connectify.Helpers;
using Connectify.Interfaces;
using Connectify.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using System.Security.Claims;

namespace Connectify.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        private readonly UserConnectionManager
            _connectionManager;

        private readonly UserManager<User>
            _identityUserManager;


        public ChatHub(
            IChatService chatService,
            UserConnectionManager connectionManager,
            UserManager<User> identityUserManager)
        {
            _chatService = chatService;

            _connectionManager = connectionManager;

            _identityUserManager = identityUserManager;
        }


        // USER CONNECTED
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var appUser =
                await _identityUserManager
                    .FindByIdAsync(userId);

            var userName =
                appUser?.FullName ?? "Unknown";


            _connectionManager.AddUser(
                Context.ConnectionId,
                userName);


            await Clients.All.SendAsync(
                "UserConnected",
                userName,
                _connectionManager.OnlineUsersCount());


            await Clients.All.SendAsync(
                "OnlineUsers",
                _connectionManager.GetOnlineUsers());


            await base.OnConnectedAsync();
        }


        // USER DISCONNECTED
        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            var appUser =
                await _identityUserManager
                    .FindByIdAsync(userId);

            var userName =
                appUser?.FullName ?? "Unknown";


            _connectionManager.RemoveUser(
                Context.ConnectionId);


            await Clients.All.SendAsync(
                "UserDisconnected",
                userName,
                _connectionManager.OnlineUsersCount());


            await Clients.All.SendAsync(
                "OnlineUsers",
                _connectionManager.GetOnlineUsers());


            await base.OnDisconnectedAsync(exception);
        }


        // SEND MESSAGE
        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;


            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;


            var appUser =
                await _identityUserManager
                    .FindByIdAsync(userId);


            var userName =
                appUser?.FullName ?? "Unknown";


            var savedMessage =
                await _chatService.SaveMessageAsync(
                    userId,
                    userName,
                    message);


            await Clients.All.SendAsync(
        "ReceiveMessage",
        savedMessage.UserId,
        savedMessage.UserName,
        savedMessage.Message,
        savedMessage.SentAt
            .ToLocalTime()
            .ToString("hh:mm tt"));
        }


        // TYPING
        public async Task Typing()
        {
            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;


            var appUser =
                await _identityUserManager
                    .FindByIdAsync(userId);


            var userName =
                appUser?.FullName ?? "Unknown";


            await Clients.Others.SendAsync(
                "UserTyping",
                userName);
        }
    }
}