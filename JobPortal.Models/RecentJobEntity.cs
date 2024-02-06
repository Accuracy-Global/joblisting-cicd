using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class RecentJobEntity
    {
        public long JobId { get; set; }
        public long EmployerId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string JobUrl { get; set; }
        public string EmployerUrl { get; set; }
        public string ExpiryDate { get; set; }
        public int MaxRows { get; set; }
    }
}
