using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using CzadRoom.Models;

namespace CzadRoom.Hubs {
    public class PublicHub : Hub {
        private readonly IPublicRoomService _publicRoomService;
        private readonly IServerCommands _serverCommands;
        private readonly IConnectionService _connectionService;

        public PublicHub(IPublicRoomService publicRoomService, IServerCommands serverCommands, IConnectionService connectionService) {
            _publicRoomService = publicRoomService;
            _serverCommands = serverCommands;
            _connectionService = connectionService;
        }

        public Task SendCommand(string message) {
            return Clients.Caller.SendAsync("ReceiveServerMessage", _serverCommands.ExecuteCommand(message));
        }

        public Task SendRoomMessage(string roomId, string message) {
            return Clients.Groups(roomId).SendAsync("ReceiveMessage", _connectionService.GetRoomConnection(Context.ConnectionId).UserID, message, roomId);
        }

        public async Task JoinRoom(string roomId, string username) {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            _connectionService.UserConnected(Context.ConnectionId, new RoomConnection { RoomID = roomId, UserID = username });
            await Clients.Group(roomId).SendAsync("ClientJoined", username);
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
