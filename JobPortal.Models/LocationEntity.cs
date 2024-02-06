using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class LocationEntity
    {
        public long CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public long StateId { get;set;}
        public string StateName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string TimeZone { get; set; }
    }
}
