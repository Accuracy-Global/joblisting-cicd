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
    public class ResumeUploadModel
    {

        public ResumeUploadModel()
        {
            this.CategoryId = SharedService.Instance.GetDefaultSpecialization().Id;
            SubSpecialization subSpecialization = SharedService.Instance.GetDefaultSubSpecialization();
            this.SpecializationId = subSpecialization.Id;
        }
        [Required(ErrorMessage = "Please provide resume title.")]
        [Display(Name = "Resume Title")]
        public string Title
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select category.")]
        [Display(Name = "Category")]
        public int CategoryId
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select specialization.")]
        [Display(Name = "Specialization")]
        public int SpecializationId
        {
            get;
            set;
        }
       
        [Required(ErrorMessage = "Please select country.")]
        [Display(Name = "Country")]
        public long? CountryId
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select state.")]
        [Display(Name = "State")]
        public long? StateId
        {
            get;
            set;
        }

        public string City { get; set; }

        [Required]
        [Display(Name = "Experience")]
        public int Experience
        {
            get;
            set;
        }

        //[Display(Name = "Current Salary")]
        //public decimal CurrentSalary
        //{
        //    get;
        //    set;
        //}
        //[Display(Name = "Expected Salary")]
        //[CompareNumerics("CurrentSalary", CompareTypes.GreaterThanEquals, ErrorMessage = "Expected Salary should be greater than equals to minimum sal.")]
        //public decimal ExpectedSalary
        //{
        //    get;
        //    set;
        //}

        [Required]
        [Display(Name = "Max")]
        public int MaximumAge
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Min")]
        public int MinimumAge
        {
            get;
            set;
        }

        [Display(Name = "Current Salary")]
        public decimal? CurrentSalary { get; set; }

        [Display(Name = "Expected Salary")]
        public decimal? ExpectedSalary
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please provide qualification.")]
        [Display(Name = "Highest Qualification")]
        public string HighestQualification { get; set; }

        [Required(ErrorMessage="Please provide resume file.")]
        [Display(Name = "Upload your resume")]
        public HttpPostedFileBase ResumeFile { get; set; }

        public IEnumerable<Specialization> Categories
        {
            get
            {
                return SharedService.Instance.GetSpecialisations();
            }
        }

        public IEnumerable<SubSpecialization> Specialisations
        {
            get
            {
                return SharedService.Instance.GetSubSpecialisations(SharedService.Instance.GetDefaultSpecialization().Id);
            }
        }

        public SelectList ExperienceList
        {
            get
            {
                SelectList explist = new SelectList(SharedService.Instance.GetExperienceList(0), "value", "key");
                return explist;
            }
        }

        public SelectList AgeList(int age = 18)
        {
            return new SelectList(SharedService.Instance.GetAgeList(age), "value", "key");
        }

        public IEnumerable<List> Qualifications
        {
            get
            {
                return SharedService.Instance.GetQualificationList();
            }
        }
        public IEnumerable<List> Countries
        {
            get
            {
                return SharedService.Instance.GetCountryList();
            }
        }
        public IEnumerable<List> States
        {
            get
            {
                return SharedService.Instance.GetStatesOfDefaultCountry();
            }
        }

    }
}