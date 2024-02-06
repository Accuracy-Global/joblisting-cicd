using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ApplicationSearchModel
    {
        public string Title { get; set; }
        public long? CountryId { get; set; }
        public string Type { get; set; }
        public string MinimumExperience { get; set; }
        public int? Status { get; set; }
        public string FromDay { get; set; }
        public string FromMonth { get; set; }
        public string FromYear { get; set; }
        public string ToDay { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
    }
}