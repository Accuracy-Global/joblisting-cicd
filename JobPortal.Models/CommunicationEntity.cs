using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class CommunicationEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ConnectedUserId { get; set; }
        public string FullName { get; set; }
        public string PermaLink { get; set; }
        public string EmailAddress { get; set; }        
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int SpecializationId { get; set; }
        public bool Downloadable { get; set; }
        public string ConnectionStatus { get; set; }
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public int BlockedByMe { get; set; }
        public int Blocked { get; set; }        
        public bool IsDeleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsConnected { get; set; }
        public bool IsBlocked { get; set; }
        public bool Initiated { get; set; }              
        public int Messages { get; set; }
        public long? InterviewId { get; set; }
        public int? InterviewStatus { get; set; }
        public int MaxRows { get; set; }
    }
}
