
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.BackGroundServices.Services;
using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices
{
    public class UserActivityBackgroundService : BackgroundService
    {
        private readonly UserActivityChannel _channel;
        private readonly ILogger<UserActivityBackgroundService> _logger;

        public UserActivityBackgroundService(UserActivityChannel channel, ILogger<UserActivityBackgroundService> logger)
        {
            _channel = channel;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("User activity background service started.");
            await Task.Delay(1000, stoppingToken);

            await foreach (var activity in _channel.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation("Admin Activity: {Action} by {UserId} at {Time}",
                    activity.action, activity.userId, activity.timeStamp);
            }

        }
    }
}
