using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    //public class IpGeolocation
    //{
    //    public string country_code { get; set; }
    //    public string country_name { get; set; }
    //    public string city { get; set; }
    //    public string postal { get; set; }
    //    public string latitude { get; set; }
    //    public string longitude { get; set; }
    //    public string IPv4 { get; set; }
    //    public string state { get; set; }
    //}

    public class IpGeolocation
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; } 
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        //public string country { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }

    public class GeoLocationError
    {
        public bool success { get; set; }
        public Error error { get; set; }
    }
}
