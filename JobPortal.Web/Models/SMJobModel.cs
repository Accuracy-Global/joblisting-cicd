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
    public class SMJobModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Select Country")]
        [Display(Name = "Country")]
        public string CountryName { get; set; }



        [Required(ErrorMessage="Provide Company Name!")]
        [Display(Name="CompanyName")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Provide Job Title!")]
        [Display(Name = "JobTitle")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Provide Mandatory Skills!")]
        [Display(Name = "Mandatory Skills")]
        public string MSkills { get; set; }

        [AllowHtml]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Description must be min. 10 and max. 2000 characters!")]
        [Required(ErrorMessage = "Provide  JobDescription min. 10 and max. 2000 characters!")]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [AllowHtml]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Responsibility must be min. 10 and max. 2000 characters!")]
        [Required(ErrorMessage = "Provide  JobResponsibility min. 10 and max. 2000 characters!")]
        [Display(Name = "Job Responsibility")]
        public string JobResponsibility { get; set; }
        [AllowHtml]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Requirement must be min. 10 and max. 2000 characters!")]
        [Required(ErrorMessage = "Provide  JobRequirement min. 10 and max. 2000 characters!")]
        [Display(Name = "Job Requirement")]
        public string JobRequirement { get; set; }


        [Required(ErrorMessage = "Provide Job Link!")]
        [Display(Name = "JobLink")]
        //[RegularExpression(@"(http://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessage = "Please Provide correct JobLink!")]

        public string JobLink { get; set; }

        [Required(ErrorMessage = "Provide Contact No!")]
        [Display(Name = "ContactNo")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Contact number")]

        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Provide  Mail")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Provide Job Source!")]
        [Display(Name = "Job Source")]
        public string JobSource { get; set; }


        
        [Display(Name = "Name")]
        public string Name { get; set; }

    }
}