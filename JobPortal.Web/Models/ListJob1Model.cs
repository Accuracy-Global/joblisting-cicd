using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ListJob1Model
    {
        [Required(ErrorMessage = "Please enter Company Name.")]
        public String CompanyName { get; set; }
       // [Required(ErrorMessage = "Please enter Company .")]
        public String AboutCompany { get; set; }
        public string CompanyLogo { get; set; }
        public int job_id { get; set; }
    }
}