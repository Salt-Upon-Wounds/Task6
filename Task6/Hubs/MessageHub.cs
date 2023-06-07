using Microsoft.AspNetCore.SignalR;

namespace Task6.Hubs
{
    public class MessageHub : Hub
    {
        public async Task Send(string username, string message)
        {
            await this.Clients.All.SendAsync("Receive", username, message);
        }
    }
}
