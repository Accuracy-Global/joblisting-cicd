using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class SMSStatusEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string StatusDate { get; set; }
        public string Status { get; set; }
    }
}
