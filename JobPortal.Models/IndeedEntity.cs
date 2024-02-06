using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class IndeedEntity
    {
        public int CategoryId { get; set; }
        public long CountryId { get; set; }
        public string CountryCode { get; set; }
        public string Category { get; set; }
    }
}
