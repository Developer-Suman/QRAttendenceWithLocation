using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices.Interface
{
    public interface IUserActivityChannel
    {
        Channel<UserActivityLogs> Channel { get; }
        ValueTask WriteAsync(UserActivityLogs activity);
        ValueTask<UserActivityLogs> ReadAsync(CancellationToken cancellationToken);
    }
}
