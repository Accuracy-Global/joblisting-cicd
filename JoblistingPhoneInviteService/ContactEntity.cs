using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingInviteSMSService
{
    public class ContactEntity
    {
        public long ContactId { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public int AlertType { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }

        public Guid? Token { get; set; }
        public string TwilioSID { get; set; }
        public string TwilioToken { get; set; }
        public string TwilioNumber { get; set; }
    }
}
