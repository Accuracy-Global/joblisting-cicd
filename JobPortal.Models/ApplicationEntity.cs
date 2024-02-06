using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ApplicationEntity
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public long? JobId { get; set; }
        public string Title { get; set; }
        public string JobUrl { get; set; }
        public long EmployerId { get; set; }
        public string EmployerEmail { get; set; }
        public string Company { get; set; }
        public string EmployerUrl { get; set; }
        public long JobseekerId { get; set; }
        public string JobseekerEmail { get; set; }
        public string JobseekerName { get; set; }
        public string JobseekerUrl { get; set; }
        public long CountryId { get;set; }
        public string CountryName { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool JobDeleted { get; set; }
        public bool BlockedByMe { get; set; }
        public bool BlockedBySomeone { get; set; }
        public long ConnectionId { get; set; }
        public bool IsConnected { get; set; }
        public string ConnectionStatus { get; set; }
        public bool InvitedByMe { get; set; }
        public bool InvitedBySomeone { get; set; }
        public bool Downloadable { get; set; }
        public int Messages { get; set; }
        public string ApplicationStatus { get; set; }
        public string StatusDate { get; set; }
        public int MaxRows { get; set; }

    }
}
