using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class CreditEntry
    {
        public long UserId { get; set; }
        public long? JobId { get; set; }
        public int PackageId { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public int? Resumes { get; set; }
        public int? Messages{get;set;}
        public int? Profiles { get; set; }
        public int? Interviews { get; set; }
        public string TransactionId { get; set; }
        public string Method { get; set; }
        public string BillingZip { get; set; }
    }
}
