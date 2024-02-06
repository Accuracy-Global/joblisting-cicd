using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PaymentInfoEntity
    {
        public int PackageId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string SecurityCode { get; set; }
        public string BillingZip { get; set; }    
        public string Username { get; set; }
        public int? Qty { get; set; }
        public decimal? Amount { get; set; }
        public string Type { get; set; }
    }

    public class PaymentModel
    {
        public string PayId { get; set; }
        public int PackageId { get; set; }
        public string Username { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
    }
}
