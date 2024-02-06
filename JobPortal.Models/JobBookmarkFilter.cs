using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class JobBookmarkFilter
    {
        public long UserId { get; set; }
        public long? EmployerId { get; set; }
        public long? CountryId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int PageNumber { get; set; }
    }
}
