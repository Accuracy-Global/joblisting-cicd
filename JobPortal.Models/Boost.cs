using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class Boost
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string Area { get; set; }
        public int Days { get; set; }
        public decimal Rate { get; set; }
    }
}
