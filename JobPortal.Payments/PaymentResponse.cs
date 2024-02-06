namespace JobPortal.Payments
{
    public class PaymentResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }
}
