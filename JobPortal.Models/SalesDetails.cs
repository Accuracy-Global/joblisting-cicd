using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class SalesDetails
    {
        public long UserId { get; set; }
        public int UserType { get; set; }
        public string FullName { get; set; }
        public string PermaLink { get; set; }
        public decimal Free { get; set; }
        public decimal Basic { get; set; }
        public decimal Advance { get; set; }        
        public decimal Total { get; set; }
        public int MaxRows { get; set; }
    }
}
