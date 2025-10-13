using QRWithSignalR.Entity;
using System.Threading.Channels;

namespace QRWithSignalR.BackGroundServices.Interface
{
    public interface IActivityChannel
    {
        ValueTask WriteAsync(UserActivityLogs activity);
        ValueTask<UserActivityLogs> ReadAsync(CancellationToken cancellationToken);

        //For Production
        void AddActivity(UserActivityLogs activity);
        IAsyncEnumerable<UserActivityLogs> ReadAllAsync(CancellationToken cancellationToken);
    }
}
