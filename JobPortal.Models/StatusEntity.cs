using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class StatusEntity
    {
        public long Id { get; set; }
        public bool Initiated { get; set; }
        public bool Accepted { get; set; }
    }
}
