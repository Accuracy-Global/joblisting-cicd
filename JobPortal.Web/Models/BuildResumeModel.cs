using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Validators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class BuildResumeModel
    {
        public long Id { get; set; }

        [Display(Name = "Profile Title")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be min. 5 and max. 50 characters!")]
        [Required(ErrorMessage = "Provide title min. 5 and max. 50 characters!")]
        [RegularExpression(@"[ A-Za-z0-9_.-]*$", ErrorMessage = "Only space and and special characters (_.-) allowed!")]
        public string Title
        {
            get;
            set;
        }


        [Required]
        [Display(Name = "Category")]        
        public int? CategoryId
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Specialization")]
        public int? SpecializationId
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Provide Summary of Experience!")]
        [Display(Name = "Summary")]
        [StringLength(2000, ErrorMessage = "Summary of Experience must be max. 2000 characters!")]
        [AllowHtml]
        public string Summary
        {
            get;
            set;
        }

        //[Required(ErrorMessage = "Provide Area Of Expertise!")]
        //[Display(Name = "Area Of Expertise")]
        //[AllowHtml]
        //public string AreaOfExpertise
        //{
        //    get;
        //    set;
        //}

        [Required(ErrorMessage = "Provide Work Experience!")]
        [Display(Name = "Professional Experience")]
        [StringLength(1500, ErrorMessage = "Work Experience must be max. 1500 characters!")]
        [AllowHtml]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Provide Skills!")]
        [Display(Name = "Skills")]
        [StringLength(2500, ErrorMessage = "Skills must be max. 2500 characters!")]
        [AllowHtml]
        public string Skills
        {
            get;
            set;
        }

        [Display(Name = "Management Skills")]
        [AllowHtml]
        public string ManagementSkills
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Provide Education & Qualification!")]
        [Display(Name = "Education & Qualification")]
        [StringLength(1500, ErrorMessage = "Education & Qualification must be max. 1500 characters!")]
        [AllowHtml]
        public string Education { get; set; }

        [Display(Name = "Professional Certification")]
        [StringLength(1500, ErrorMessage = "Professional Certification must be max. 1500 characters!")]
        [AllowHtml]
        public string Certifications { get; set; }

        [Display(Name = "Professional Affiliation")]
        [StringLength(1500, ErrorMessage = "Professional Affiliation must be max. 1500 characters!")]
        [AllowHtml]
        public string Affiliations { get; set; }

        public string ReturnUrl { get; set; }
    }
}