using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Promote
    {
        public long Id { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public int PackageId { get; set; }
        public decimal RatePerDay { get; set; }
        public int Days { get; set; }
        public int Limit { get; set; }
        
    }
}
