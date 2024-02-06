using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class InterviewEntity
    {
        public long Id { get; set; }
        public Guid TrackingId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewTime { get; set; }
        public string InterviewStatus { get; set; }
        public int Round { get; set; }
        public int Status { get; set; }
        public long? JobId { get; set; }
        public string Title { get; set; }
        public string JobUrl { get; set; }

        public long EmployerId { get; set; }
        public string Company { get; set; }
        public string EmployerUrl { get; set; }
        public string EmployerEmail { get; set; }
        public long? JobseekerId { get; set; }
        public string Name { get; set; }
        public string JobseekerUrl { get; set; }
        public string JobseekerEmail { get; set; }
        public bool Downloadable { get; set; }
        public int InterviewCounts { get; set; }
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
