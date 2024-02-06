using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class GlobalInboxEntity
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Individuals { get; set; }
        public int Companies { get; set; }
        public int Total { get; set; }
        public int MaxRows { get; set; }

    }
}
