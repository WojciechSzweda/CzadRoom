using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CzadRoom.ViewModels;
using CzadRoom.Extensions;

namespace CzadRoom.Hubs {
    [Authorize]
    public class ChatHub : Hub {
        private readonly IRoomService _roomService;
        private readonly IServerCommands _serverCommands;
        private readonly IChatMessageService _chatMessageService;
        private readonly IConnectionService _connectionService;

        public ChatHub(IRoomService roomService, IServerCommands serverCommands, IChatMessageService chatMessageService, IConnectionService connectionService) {
            _roomService = roomService;
            _serverCommands = serverCommands;
            _chatMessageService = chatMessageService;
            _connectionService = connectionService;
        }

        public async Task SendMessage(string message) {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public Task SendCommand(string message) {
            return Clients.Caller.SendAsync("ReceiveServerMessage", _serverCommands.ExecuteCommand(message));
        }

        public Task SendRoomMessage(string roomId, string message) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_roomService.HasUserAccess(roomId, userId)) {
                var msg = new ChatMessage { Content = message, RoomID = roomId, UserID = userId, Username = Context.User.Identity.Name };
                _chatMessageService.AddMessage(msg);
                return Clients.Groups(roomId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, roomId);
            }
            else {
                return Clients.Caller.SendAsync("ReceiveServerMessage", message);
            }
        }

        public async Task JoinRoom(string roomId) {
            var userId = Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (_roomService.HasUserAccess(roomId, userId))
                await Clients.Caller.SendAsync("ReceiveServerMessage", "error");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            _connectionService.UserConnected(Context.ConnectionId, new RoomConnection { RoomID = roomId, UserID = userId });
            await Clients.Group(roomId).SendAsync("ClientJoined", userId, Context.User.Identity.Name);
        }

        public override async Task OnConnectedAsync() {
            await Clients.Caller.SendAsync("Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            var (isDisconnected, connection) = _connectionService.UserDisconnected(Context.ConnectionId);
            if (isDisconnected)
                await Clients.Group(connection.RoomID).SendAsync("ClientLeft", Context.User.Identity.Name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
