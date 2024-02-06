using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class PhoneContactEntity
    {
        public long? Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string DeviceId { get; set; }

    }
}
