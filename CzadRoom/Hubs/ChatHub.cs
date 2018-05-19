using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IRoomService _roomService;
        public ChatHub(IRoomService roomService) {
            _roomService = roomService;
        }

        public async Task SendMessage(string message) {
            
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name , message);
        }

        public async Task SendRoomMessage(string roomID, string message) {

            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public Task SendMessageToCaller(string message) {
            return Clients.Caller.SendAsync("ReceiveServerMessage",  message);
        }

        public Task SendMessageToGroup(string message) {
            List<string> groups = new List<string>() { "Group1" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public async Task JoinRoom(string roomId) {
            //TODO: add user to room in db, so he can be removed when d/c
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ClientJoined", Context.User.Identity.Name);
        }

        public async Task LeaveRoom(string roomId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ClientJoined", Context.User.Identity.Name);
        }

        public override async Task OnConnectedAsync() {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Group1");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Group1");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
