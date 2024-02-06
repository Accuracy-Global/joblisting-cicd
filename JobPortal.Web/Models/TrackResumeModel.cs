using JobPortal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class TrackResumeModel
    {
        [Display(Name="Job Title")]
        public long? JobId { get; set; }

        [Display(Name = "Company")]
        public long? EmployerId { get; set; }

        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        public List<Tracking> Applications { get; set; }
    }
}