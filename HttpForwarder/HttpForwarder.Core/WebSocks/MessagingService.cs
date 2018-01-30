using HttpForwarder.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HttpForwarder.Core.WebSocks
{
    public class MessagingService
    {
        IHubContext<ClientSub> _hubContext;
        public MessagingService(IHubContext<ClientSub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendMessage(string groupId, FwdRequest message)
        {
            return _hubContext.Clients.Group(groupId).InvokeAsync("Send", message);
        }
    }
}