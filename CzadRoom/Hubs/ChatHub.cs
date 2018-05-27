using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CzadRoom.Hubs {
    [Authorize]
    public class ChatHub : Hub {
        private readonly IRoomService _roomService;
        public ChatHub(IRoomService roomService) {
            _roomService = roomService;
        }

        public async Task SendMessage(string message) {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public Task SendMessageToCaller(string message) {
            return Clients.Caller.SendAsync("ReceiveServerMessage", message);
        }

        public Task SendRoomMessage(string roomId, string message) {
            if (_roomService.HasUserAccess(roomId, Context.User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Clients.Groups(roomId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, roomId);
            else {
                return Clients.Caller.SendAsync("ReceiveServerMessage", message);
            }
        }

        public async Task JoinRoom(string roomId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ClientJoined", Context.User.Identity.Name);
        }

        public async Task LeaveRoom(string roomId) {
            var clientId = Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await _roomService.UserDisconnected(roomId, clientId);
            await Clients.Group(roomId).SendAsync("ClientLeft", Context.User.Identity.Name);
        }

        public override async Task OnConnectedAsync() {
            await Clients.Caller.SendAsync("Connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
