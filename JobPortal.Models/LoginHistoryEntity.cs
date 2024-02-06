using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class LoginHistoryEntity
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public System.DateTime LoginDateTime { get; set; }
        public string Browser { get; set; }
        public string Comments { get; set; }
        public int MaxRows { get; set; }
    }
}
