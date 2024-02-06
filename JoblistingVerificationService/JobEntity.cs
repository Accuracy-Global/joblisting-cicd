using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingVerificationService
{
    public class JobEntity
    {
        public long Id { get; set; }
        public long EmployerId { get; set; }
        public string Title { get; set; }
        public string PermaLink { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Live { get; set; }
        public int Expired { get; set; }
        public int Waiting { get; set; }
        public int Rejected { get; set; }
        public int Deactivated { get; set; }
        public int Deleted { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public int MaxRows { get; set; }
    }
}
