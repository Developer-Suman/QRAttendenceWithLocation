namespace QRWithSignalR.ServiceDTOs
{
    public class PurchaseDTOs
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        public string? Role { get; set; }
    }
}
