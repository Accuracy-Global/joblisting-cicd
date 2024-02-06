using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class IndeedJob
    {
        public string Source { get; set; }
        public string JobTitle { get; set; }
        public string Snippet { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public bool Sponsored { get; set; }
        public bool Expired { get; set; }
        public string Url { get; set; }
        public string JobKey { get; set; }
        public int CategoryId { get; set; }
        public long CountryId { get; set; }

    }
}
