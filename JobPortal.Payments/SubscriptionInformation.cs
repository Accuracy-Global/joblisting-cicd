namespace JobPortal.Payments
{
    public class SubscriptionInformation
    {
        public string PayerId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string PaymentId { get; set; }
        public int Quantity { get; set; } = 1;
        public string Currency { get; set; } = "USD";
        public string SKU { get; set; } = "sku";
    }
}
