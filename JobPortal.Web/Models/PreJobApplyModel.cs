using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class PreJobApplyModel
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Job title must be min. 5 and max. 50 characters!")]
        [Required(ErrorMessage = "Provide title min. 5 and max. 50 characters!")]
        [Display(Name="Title")]
        public string Title { get; set; }

        [Required(ErrorMessage="Provide category!")]
        [Display(Name="Category")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage="Provide specialization")]
        [Display(Name = "Specialization")]
        public int? SpecializationId { get; set; }

        [Display(Name = "Country Code")]
        public string MobileCountryCode
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Provide mobile number!")]
        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Mobile Number must be Min. 6 and Max. 15 numbers!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Telephone
        {
            get;
            set;
        }
        
        [DataType(DataType.Text)]
        [Display(Name = "Current Employer")]
        public string CurrentEmployer { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "From Month")]
        public string FromMonth { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "To Year")]
        public string FromYear { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "To Month")]
        public string ToMonth { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "To Year")]
        public string ToYear { get; set; }

        [Required(ErrorMessage = "Provide Summary of Experience!")]
        [Display(Name = "Summary")]
        [StringLength(2000, ErrorMessage = "Summary of Experience must be max. 2000 characters!")]
        [AllowHtml]
        public string Summary
        {
            get;
            set;
        }

        public HttpPostedFileBase Resume { get; set; }

        public string ReturnUrl { get; set; }
        public string Type { get; set; }
        public Hashtable Countries
        {
            get
            {
                List<List> countrylist = SharedService.Instance.GetCountryList();
                Hashtable countries = new Hashtable();
                foreach (var item in countrylist)
                {
                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        countries.Add(item.Id, string.Format("+{0}", item.Code));
                    }
                }
                return countries;
            }
        }
    }
}