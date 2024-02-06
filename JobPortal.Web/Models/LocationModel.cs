using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class LocationModel
    {
        public string IP { get; set; }
        public string Country_Code { get; set; }
        public string Country { get; set; }
        public string Region_Code { get; set; }
        public string region { get; set; }
        public string City { get; set; }
        public string Zip_Code { get; set; }
        public string Time_Zone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Metro_Code { get; set; }
    }
}