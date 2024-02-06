using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public sealed class Companies
    {
        public long UserId { get; set; }
        public string Company { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Status { get; set; }
        public string jurl { get; set; }
    }
}
