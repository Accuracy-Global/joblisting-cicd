using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
   
    public sealed class Jobseekers
    {
        public long JobID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public string ImageType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Mskilles { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Status { get; set; }
        public long TotalViews { get; set; }
        public string JobType { get; set; }
        public string MinimumExperience { get; set; }
        public string JobLevel { get; set; }
        public long EmployerId { get; set; }
        public long TotalRow { get; set; }
        public string JobURL { get; set; }
        public string Qualification { get; set; }
    }
}
