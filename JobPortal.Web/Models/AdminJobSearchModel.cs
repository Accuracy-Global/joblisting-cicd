using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class AdminJobSearchModel
    {
        public long? CountryId { get; set; }
        public long? Id {get;set;}
        public string JobTitle { get; set; }
        public int? TypeId{get;set;}
      
        public string fd{get;set;}
        public string fm{get;set;}
        public string fy{get;set;}

        public string td{get;set;}
        public string tm{get;set;}
        public string ty{get;set;}
    }
}