using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class BuyPromote
    {
        public long Id { get; set; }
        public int PackageId { get; set; }
        public string Type { get; set; }        
        public int Days { get; set; }
        public decimal TotalAmount { get; set; }

        public string ReturnUrl { get; set; }

        public string Description { get; set; }

        public Guid? SessionId { get; set; }
    }
}
