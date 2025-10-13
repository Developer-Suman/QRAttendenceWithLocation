namespace QRWithSignalR.Entity
{
    public record UserActivityLogs
    (
        string userId,
        string action,
        DateTime timeStamp
        );
}
