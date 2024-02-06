using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PromoteItem
    {
        public long Id { get; set; }
        public int PackageId { get; set; }
        public int Days { get; set; }
        public string TransactionId { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
    }
}
