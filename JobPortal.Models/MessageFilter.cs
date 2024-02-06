using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class MessageFilter
    {
        public long? UserId { get; set; }
        public long? ReceiverId { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
