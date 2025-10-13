using Microsoft.AspNetCore.SignalR;
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.BackGroundServices.Services;
using QRWithSignalR.SignalRHub;

namespace QRWithSignalR.BackGroundServices
{
    public class AdminActivityBackgroundServices : BackgroundService
    {
        private readonly AdminActivityChannel _channel;
        private readonly ILogger<AdminActivityBackgroundServices> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AdminActivityBackgroundServices(AdminActivityChannel channel, ILogger<AdminActivityBackgroundServices> logger, IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            _channel = channel;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Admin Activity Background Service started.");

            await foreach (var activity in _channel.ReadAllAsync(stoppingToken))
            {
                // Send notification to all clients
                await _hubContext.Clients.All.SendAsync(
                      "AdminActivity",
                      $"✅ New Activity Done: UserId = {activity.UserId}, Action = {activity.UserAgent}, Time = {activity.TimeStamp}"
                  );

                await _hubContext.Clients.All.SendAsync(
                   "AdminActivity",
                   $"✅ New Activity Done: UserId = {activity.IpAddress}"
               );

                _logger.LogInformation(" => UserId: {UserId}, Action: {Action}, Time: {Time}",
                    activity.UserId, activity.Action, activity.TimeStamp);

                // TODO: You can save to DB or broadcast via SignalR here
            }
        }
    }
}
