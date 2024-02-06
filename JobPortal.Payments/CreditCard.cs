using PayPal.Api;

namespace JobPortal.Payments
{
    public class CreditCard
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string SecurityCode { get; set; }
        public string Amount { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}
