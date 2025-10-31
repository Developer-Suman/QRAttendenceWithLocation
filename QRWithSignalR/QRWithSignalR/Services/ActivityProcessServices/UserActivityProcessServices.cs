using Microsoft.AspNetCore.SignalR;
using QRWithSignalR.Entity;
using QRWithSignalR.SignalRHub;
using System.Threading;

namespace QRWithSignalR.Services.ActivityProcessServices
{
    public class UserActivityProcessServices
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<UserActivityProcessServices> _logger;

        public UserActivityProcessServices(IHubContext<NotificationHub> hubContext, ILogger<UserActivityProcessServices> logger)
        {
            _hubContext = hubContext;
            _logger = logger;

        }

        public async Task ProcessActivityAsync( IAsyncEnumerable<UserActivityLogs> activity, CancellationToken cancellationToken)
        {

            try
            {
                await foreach (var activities in activity.WithCancellation(cancellationToken))
                {
                    // Send notification to all clients
                    await _hubContext.Clients.All.SendAsync(
                      "AdminActivity",
                      $"✅ New Activity Done: UserId = {activities.UserId}, Action = {activities.UserAgent}, Time = {activities.TimeStamp}"
                  );
         

                }

            }
            catch(Exception ex)
            {
                throw new Exception();

            }

        
               
     
        }
    }
}
