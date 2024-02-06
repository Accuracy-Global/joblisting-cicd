using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ConnectionsByCountryModel
    {
        public long CountryId { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }

        public int Individuals { get; set; }
        public int Companies { get; set; }
        public int Users { get; set; }
    }
}