using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class UserFilter
    {
        public long? UserId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string CountryId { get; set; }
    }
}
