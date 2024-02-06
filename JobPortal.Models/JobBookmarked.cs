using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class JobBookmarked
    {
        public Guid Id { get; set; }
        public long JobId { get; set; }
        public string Title { get; set; }
        public string JobUrl { get; set; }
        public long UserId { get; set; }
        public string Company { get; set; }
        public string EmployerUrl { get; set; }
        public string Country { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public long ConnectionId { get; set; }
        public bool IsConnected { get; set; }
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public int BlockedByMe { get; set; }
        public int Blocked { get; set; }
        public int Messages { get; set; }
        public int MaxRows { get; set; }
    }
}
