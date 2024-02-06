using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class MessageModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public bool Unread { get; set; }
        public bool IsSent { get; set; }
        public bool IsReply { get; set; }
        public bool IsInitial { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string UpdatedBy { get; set; }
        public string DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public long UserId { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Side { get; set; }
    }
}
