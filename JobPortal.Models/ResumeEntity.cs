using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ResumeEntity
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string PermaLink { get; set; }
        public string Category { get; set; }
        public string Specialization { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public DateTime DateUpdated { get; set; }

        public int MaxRows { get; set; }
    }
}
