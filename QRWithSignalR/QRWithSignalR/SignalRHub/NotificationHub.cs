using Microsoft.AspNetCore.SignalR;

namespace QRWithSignalR.SignalRHub
{
    public class NotificationHub : Hub
    {
        public async Task BroadcastPurchaseNotification(string message)
        {
            await Clients.All.SendAsync("Receive Notification", message);
        }
    }
}
