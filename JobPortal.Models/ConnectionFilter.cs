using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ConnectionFilter
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
