using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "Provide Name on Card")]
        [Display(Name = "Name on Card")]
        public string HolderName { get; set; }
        
        [Display(Name="Card Number")]
        [Required(ErrorMessage = "Provide Credit Card Number")]
        [StringLength(maximumLength: 19, MinimumLength = 15)]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Provide Expiry Month")]
        public string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Provide Expiry Year")]
        public string ExpiryYear { get; set; }

        [Required(ErrorMessage = "Provide Card CVV")]
        [Display(Name="CVV")]
        [DataType(DataType.Password)]
        public string SecurityCode { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Provide PayPal Email")]
        public string PayeeEmail { get; set; }
        public string RUrl { get; set; }
        public int PackageId { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }

        // 0 - Credit Card, 1 - Pay Pal
        public int PaymentMode { get; set; }

        public Guid? SessionId { get; set; }
        public String Type { get; set; }

        public long Id { get; set; }        
        public int Days { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Provide alphanumeric values.")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Billing Postal Code (ZIP)")]
        public string BillingZip { get; set; }
    }
}