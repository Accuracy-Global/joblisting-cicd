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
    public class CampaignModel
    {
        public long Id { get; set; }

        [Display(Name="Type")]
        [Required(ErrorMessage="Provide Type")]
        public int Type { get; set; }
        [Required(ErrorMessage = "Select Country")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Select category")]
        [Display(Name = "Job Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Select specialization.")]
        [Display(Name = "Specialization")]
        public int SpecializationId { get; set; }


        [Required(ErrorMessage = "Select Name.")]
        [Display(Name = "Name")]
        public string Name{ get; set; }

        [Required(ErrorMessage="Provide Subject!")]
        [RegularExpression(@"[ A-Za-z0-9(_)'?,.-]*$", ErrorMessage = "Special characters not allowed!")]
        [Display(Name="Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Provide Body!")]
        [Display(Name = "Body")]
        [AllowHtml]
        public string Body { get; set; }

        public string City { get; set; }


    }
}