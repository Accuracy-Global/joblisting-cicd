using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class JobViewModel
    {
        public long JobId { get; set; }

        public long EmployerId { get; set; }

        public long JobseekerId { get; set; }
        
        public DateTime ViewedOn { get; set; }

        public int Times { get; set; }

        public string Title { get; set; }
        public string Company { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public string Specialization { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public string Qualification { get; set; }

        public string JobType { get; set; }

        public string PermaLink { get; set; }

        public string ProfileUrl { get; set; }
        public string Logo { get; set; }

        public string ExpiryDate { get; set; }
       
    }
}