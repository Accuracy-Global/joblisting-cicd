using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ViewedEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string PermaLink { get; set; }
        public long CountryId { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public long? ConnectionId { get; set; }
        public bool? IsConnected { get; set; }
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public int BlockedByMe { get; set; }
        public int Blocked { get; set; }
        public int Messages { get; set; }
        public int MaxRows { get; set; }
        public int Views { get; set; }
    }
}
