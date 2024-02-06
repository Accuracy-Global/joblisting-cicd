using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Agreement
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string DeviceId { get; set; }
        public bool Accepted { get; set; }
    }
}
