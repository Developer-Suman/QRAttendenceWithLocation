
using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.BackGroundServices.Services;
using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices
{
    public class UserActivityBackgroundService : BackgroundService
    {
        private readonly IUserActivityChannel _activityChannel;
        private readonly ILogger<UserActivityBackgroundService> _logger;

        public UserActivityBackgroundService(IUserActivityChannel channel, ILogger<UserActivityBackgroundService> logger)
        {
            _activityChannel = channel;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("User activity background service started.");
            await Task.Delay(1000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {

                // Save to database, or publish via SignalR to admin dashboard
                var activity = await _activityChannel.ReadAsync(stoppingToken);
                _logger.LogInformation($"Processing activity: {activity.userId} - {activity.action}");
                // Example: save to DB
                // await _dbContext.UserActivityLogs.AddAsync(log);
                // await _dbContext.SaveChangesAsync();



            }
       
        }
    }
}
