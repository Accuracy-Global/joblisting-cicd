using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ContactByCountry
    {
        public long CountryId { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public int Contacts { get; set; }
    }
}
