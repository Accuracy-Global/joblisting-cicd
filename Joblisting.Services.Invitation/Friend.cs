using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingReminderService
{
    public class Friend
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string PermaLink { get; set; }
        public long ConnectionId { get; set; }
        public string EmailAddress { get; set; }
        public string CFirstName { get; set; }
        public string CLastName { get; set; }
        public int Registered { get; set; }
        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
