using QRWithSignalR.BackGroundServices.Interface;
using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices.Services
{
    public class AdminActivityChannel : IActivityChannel
    {
        public Channel<UserActivityLogs> Channel { get; }

        public AdminActivityChannel()
        {
            Channel = System.Threading.Channels.Channel.CreateUnbounded<UserActivityLogs>();
        }
        public void AddActivity(UserActivityLogs activity)
        {
            Channel.Writer.TryWrite(activity);
        }

        public async IAsyncEnumerable<UserActivityLogs> ReadAllAsync(CancellationToken cancellationToken)
        {
            await foreach (var item in Channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return item;
            }
        }

        public ValueTask<UserActivityLogs> ReadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask WriteAsync(UserActivityLogs activity)
        {
            throw new NotImplementedException();
        }
    }
}
