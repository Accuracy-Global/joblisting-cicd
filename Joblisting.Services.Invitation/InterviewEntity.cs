using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingReminderService
{
    public class InterviewEntity
    {
        public long Id { get; set; }
        public string Interviewer { get; set; }
        public int Round { get; set; }
        public DateTime InterviewDate { get; set; }
        public long EmployerId { get; set; }
        public string EmployerEmail { get; set; }
        public string EmployerName { get; set; }
        public string EmployerUrl { get; set; }

        public long JobseekerId { get; set; }
        public string JobseekerEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobseekerUrl { get; set; }

        public long? JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobUrl { get; set; }

        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
        public int FollowUpStatus { get; set; }
        public int UserType { get; set; }
    }
}
