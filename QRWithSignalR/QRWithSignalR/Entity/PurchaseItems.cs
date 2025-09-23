namespace QRWithSignalR.Entity
{
    public class PurchaseItems
    {
        public string Id { get; set; }
        public string? ItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        public string? QRUrl { get; set; }
    }
}
