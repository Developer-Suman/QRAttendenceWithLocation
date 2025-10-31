using QRWithSignalR.Entity;

namespace QRWithSignalR.BackGroundServices.Interface
{
    public interface IOAuthService
    {
        Task<string> VerifyGoogleTokenAsync(string token);
        Task<string> VerifyFacebookTokenAsync(string token);
    }
}
