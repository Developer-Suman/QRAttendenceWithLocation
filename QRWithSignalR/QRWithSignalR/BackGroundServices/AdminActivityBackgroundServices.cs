using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.BackGroundServices.Services;

namespace QRWithSignalR.BackGroundServices
{
    public class AdminActivityBackgroundServices : BackgroundService
    {
        private readonly AdminActivityChannel _channel;
        private readonly ILogger<AdminActivityBackgroundServices> _logger;

        public AdminActivityBackgroundServices(AdminActivityChannel channel, ILogger<AdminActivityBackgroundServices> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Admin Activity Background Service started.");

            await foreach (var activity in _channel.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation("ADMIN ACTIVITY => UserId: {UserId}, Action: {Action}, Time: {Time}",
                    activity.userId, activity.action, activity.timeStamp);

                // TODO: You can save to DB or broadcast via SignalR here
            }
        }
    }
}
