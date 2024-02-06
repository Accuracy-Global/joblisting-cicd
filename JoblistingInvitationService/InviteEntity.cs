using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoblistingInvitationService
{
    public class InviteEntity
    {
        public string SenderName { get; set; }
        public string SenderProfileUrl { get; set; }
        public long ConnectionId { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public int Registered { get; set; }
        public string Template { get; set; }
        public string BaseUrl { get; set; }
    }
}
