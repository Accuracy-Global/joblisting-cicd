using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Blocked
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string PermaLink { get; set; }
        public string Country { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
    }
}
