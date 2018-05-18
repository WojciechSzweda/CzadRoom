using CzadRoom.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Hubs
{
    public class ChatHub : Hub
    {
        //private readonly IChatService _chatService;
        //public ChatHub(IChatService chatService) {
        //    _chatService = chatService;
        //}

        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message) {
            return Clients.Caller.SendAsync("ReceiveServerMessage",  message);
        }

        public Task SendMessageToGroup(string message) {
            List<string> groups = new List<string>() { "Group1" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
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
