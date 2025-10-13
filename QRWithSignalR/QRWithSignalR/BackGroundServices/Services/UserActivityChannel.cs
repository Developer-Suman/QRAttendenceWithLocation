using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices.Services
{
    public class UserActivityChannel : IUserActivityChannel
    {
  
        public Channel<UserActivityLogs> Channel { get; }

        public UserActivityChannel()
        {
            Channel = System.Threading.Channels.Channel.CreateUnbounded<UserActivityLogs>();
        }

       

        public async ValueTask<UserActivityLogs> ReadAsync(CancellationToken cancellationToken)
        {
            return await Channel.Reader.ReadAsync(cancellationToken);
        }

        public async ValueTask WriteAsync(UserActivityLogs activity)
        {
            await Channel.Writer.WriteAsync(activity);
        }
    }
}
