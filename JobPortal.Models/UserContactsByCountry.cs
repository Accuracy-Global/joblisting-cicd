using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserContactsByCountry
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string PermaLink { get; set; }
        public int Contacts { get; set; }
        public int MaxRows { get; set; }
    }
}
