using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class JobDetailByCountryEntity
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int Live { get; set; }
        public int Expired { get; set; }
        public int Waiting { get; set; }
        public int Rejected { get; set; }
        public int Deactivated { get; set; }
        public int Deleted { get; set; }
        public int Applications { get; set; }
        public int MaxRows { get; set; }
    }
}
