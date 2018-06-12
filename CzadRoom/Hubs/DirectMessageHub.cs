using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CzadRoom.Services.Interfaces;
using CzadRoom.Models;

namespace CzadRoom.Hubs {
    public class DirectMessageHub : Hub {
        private readonly IDirectMessageRoomService _directMessageRoomService;
        private readonly IDirectMessageService _directMessageService;
        private readonly IConnectionService _connectionService;

        public DirectMessageHub(IDirectMessageRoomService directMessageRoomService, IDirectMessageService directMessageService, IConnectionService connectionService) {
            _directMessageRoomService = directMessageRoomService;
            _directMessageService = directMessageService;
            _connectionService = connectionService;
        }

        public Task SendRoomMessage(string roomId, string message) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_directMessageRoomService.HasUserAccess(roomId, userId)) {
                var msg = new DirectMessage { Content = message, RoomID = roomId, FromID = userId };
                _directMessageService.AddDirectMessage(msg);
                return Clients.Groups(roomId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, roomId);
            }
            else {
                return Clients.Caller.SendAsync("ReceiveServerMessage", message);
            }
        }

        public Task Focused(string roomId, bool isFocused) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_directMessageRoomService.HasUserAccess(roomId, userId))
                return Clients.Groups(roomId).SendAsync("Focused", userId, isFocused);
            return Clients.Caller.SendAsync("FocusedError", "error");
        }

        public async Task JoinRoom(string roomId) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!_directMessageRoomService.HasUserAccess(roomId, userId))
                await Clients.Caller.SendAsync("ReceiveServerMessage", "error");
            _connectionService.UserConnected(Context.ConnectionId, new RoomConnection { RoomID = roomId, UserID = userId });
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public override async Task OnConnectedAsync() {
            await Clients.Caller.SendAsync("Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            var (isDisconnected, connection) = _connectionService.UserDisconnected(Context.ConnectionId);
            if (isDisconnected)
                await Clients.Group(connection.RoomID).SendAsync("ClientLeft", connection.UserID);
            await base.OnDisconnectedAsync(exception);
        }
    }

}
