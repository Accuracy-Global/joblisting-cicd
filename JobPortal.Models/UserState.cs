using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserState
    {
        public long UserId { get; set; }
        public string UserRole { get; set; }
        public long MemberId { get; set; }
        public string MemberRole { get; set; }
        public bool IsConnected { get; set; }
        public long ConnectionId { get; set; }
        public bool Blocked { get; set; }
        public bool BlockedByMe { get; set; }
        public bool Invited { get; set; }
        public bool InvitedByMe { get; set; }
        public int Messages { get; set; }

    }
}
