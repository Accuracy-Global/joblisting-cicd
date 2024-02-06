using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PeopleEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int Type {get;set;}
        public string TypeName { get; set; }
        public byte? Age { get; set; }
        public string Gender { get; set; }
        public int? RelationshipStatus { get; set; }
        public string Content { get; set; }
        public string PermaLink { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public int? SpecializationId { get; set; }
        public long? Connected { get;set;}
		public int? Accepted{get;set;}
		public int BlockedByMe{get;set;}
		public int Blocked{get;set;}
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public int Messages { get; set; }
        public long? InterviewId { get; set; }
        public int? InterviewStatus { get; set; }
        public bool Downloadable { get; set; }
        public int? MaxRows { get; set; }
        public bool IsJobseeker { get; set; }
    }
}
