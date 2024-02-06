using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class TrackingEntity
    {
        public Guid Id { get; set; }
        public Nullable<long> ResumeId { get; set; }
        public Nullable<long> JobId { get; set; }
        public int Type { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Comments { get; set; }
        public Nullable<decimal> Weightage { get; set; }
        public bool IsDeleted { get; set; }
        public long UserId { get; set; }
        public Nullable<long> JobseekerId { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsMessageSent { get; set; }
        public bool IsInvited { get; set; }
    }
}
