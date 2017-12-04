using HttpForwarder.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpForwarder.Core.WebSocks
{
    public class ClientSub : Hub
    {
        public Task Send(string uid, HttpRequest message)
        {
            return Clients.Group(uid).InvokeAsync("Send", message);
        }
        
        public async Task Join(string uid)
        {
            await Groups.AddAsync(Context.ConnectionId, uid);            
        }

        public override async Task OnConnectedAsync()
        {
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
        }
    }
}