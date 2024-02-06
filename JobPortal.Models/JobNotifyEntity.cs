using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class JobNotifyEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int Jobs { get; set; }

    }
}
