using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Clients.Groups(roomId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, roomId);
        }

        public async Task JoinRoom(string roomId) {
            //TODO: add user to room in db
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ClientJoined", Context.User.Identity.Name);
        }

        public async Task LeaveRoom(string roomId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
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
