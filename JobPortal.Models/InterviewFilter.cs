using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class InterviewFilter
    {
        public long UserId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public long? CountryId { get; set; }
        public int? Status { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int PageNumber { get; set; }
    }
}
