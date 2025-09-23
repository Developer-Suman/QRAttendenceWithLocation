using QRWithSignalR.ServiceDTOs;

namespace QRWithSignalR.Interface
{
    public interface IPurchaseServices
    {
        Task AddPurchaseAsync(PurchaseDTOs purchase);

        Task CreateNewQR(string purchase);
    }
}
