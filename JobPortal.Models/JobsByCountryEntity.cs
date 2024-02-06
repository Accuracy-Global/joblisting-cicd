using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class JobsByCountryEntity
    {
        public string Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int Jobs { get; set; }
        public int JobsInApproval { get; set; }
        public int Applications { get; set; }
        public int MaxRows { get; set; }
    }
}
