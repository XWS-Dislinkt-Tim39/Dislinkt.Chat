using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;      // using this
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public_Chat.Hubs
{
    [Authorize]
    public class ChatHub : Hub        // inherit this
    {

       /* public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }*/
        public Task SendMessage1(string user, string message)               // Two parameters accepted
        {
            return Clients.All.SendAsync("ReceiveOne", user, message);    // Note this 'ReceiveOne' 
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
