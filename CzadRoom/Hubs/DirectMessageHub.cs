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

        public DirectMessageHub(IDirectMessageRoomService directMessageRoomService, IDirectMessageService directMessageService) {
            _directMessageRoomService = directMessageRoomService;
            _directMessageService = directMessageService;
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

        public Task ReadMessages(string roomId) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_directMessageRoomService.HasUserAccess(roomId, userId))
                return Clients.Groups(roomId).SendAsync("ReadConfirm", Context.User.Identity.Name, DateTime.Now);
            return Clients.Caller.SendAsync("ReceiveServerMessage", "error");
        }

        public async Task JoinRoom(string roomId) {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!_directMessageRoomService.HasUserAccess(roomId, userId))
                await Clients.Caller.SendAsync("ReceiveServerMessage", "error");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public override async Task OnConnectedAsync() {
            await Clients.Caller.SendAsync("Connected");
            await base.OnConnectedAsync();
        }
    }

}
