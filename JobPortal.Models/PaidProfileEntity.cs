using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PaidProfileEntity
    {
        public long UserId { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public long CountryId { get; set; }
        public string PermaLink { get; set; }
        public int MaxRows { get; set; }
    }
}
