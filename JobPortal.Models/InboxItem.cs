using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class InboxItem
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public long UserId { get; set; }
        public DateTime ActivityDate { get; set; }
        public Nullable<long> ReferenceId { get; set; }
        public string Comments { get; set; }
        public DateTime DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool Unread { get; set; }
        

    }
}
