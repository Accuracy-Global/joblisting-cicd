using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class MatchEntity
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string TypeName { get; set; }
        public long JobId { get; set; }
        public string JobTitle { get; set; }
        public string EntityName { get; set; }
        public string ApplicationStatus { get; set; }
        public string Country { get; set; }
        public int MaxRows { get; set; }
        public long Connected { get; set; }
        public bool IsConnected { get; set; }
        public int Messages { get; set; }
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public int BlockedByMe { get; set; }
        public int BlockedBySomeone { get; set; }
        
    }
}
