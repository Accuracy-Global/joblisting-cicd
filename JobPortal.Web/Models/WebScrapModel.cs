using System;
using System.Collections.Generic;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Data;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class WebScrapModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Select Country")]
        [Display(Name = "Country")]
        public  string  CountryName { get; set; }



        [Required(ErrorMessage = "Provide Company Name!")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Provide Website!")]
        //  [RegularExpression(@"(http://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessage = "Please Provide correct Website!")]
        [Display(Name = "Website")]
        public string Website { get; set; }
        public Category1 Category { get; set; }



        [Required(ErrorMessage = "Select Mail Id to Send")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Provide Name!")]
        public string Name { get; set; }
        
        public byte[] CompanyLogo { get; set; }
        [Required(ErrorMessage = "Provide Website!")]
        [Display(Name = "Logo Link")]
        public string companylogoslink { get; set; }
        public string Logo { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public HttpPostedFileBase file { get; set; }

    }

   
}

public enum Category1
{
    FullTime,
    Fresher_Job,
    PartTime,    
    Freelance,
    Internship

}