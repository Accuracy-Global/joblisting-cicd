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
    public class UploadResumeModal
    {
        public UploadResumeModal()
        {
           
        }

        [Required(ErrorMessage = "Please provide profile title.")]
        [Display(Name = "Title")]
        public string TitleOne
        {
            get;
            set;
        }
        
       
        [Display(Name = "Title")]
        public string TitleTwo
        {
            get;
            set;
        }

       
        [Display(Name = "Title")]
        public string TitleThree
        {
            get;
            set;
        }

       
        [Display(Name = "Title")]
        public string TitleFour
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select category.")]
        [Display(Name = "Category")]
        public int CategoryIdOne
        {
            get;
            set;
        }

        
        [Display(Name = "Category")]
        public int CategoryIdTwo
        {
            get;
            set;
        }

       
        [Display(Name = "Category")]
        public int CategoryIdThree
        {
            get;
            set;
        }

       
        [Display(Name = "Category")]
        public int CategoryIdFour
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select specialization.")]
        [Display(Name = "Specialization")]
        public int SpecializationIdOne
        {
            get;
            set;
        }


        [Display(Name = "Specialization")]
        public int SpecializationIdTwo
        {
            get;
            set;
        }


        [Display(Name = "Specialization")]
        public int SpecializationIdThree
        {
            get;
            set;
        }

        [Display(Name = "Specialization")]
        public int SpecializationIdFour
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select country.")]
        [Display(Name = "Country")]
        public long? CountryIdOne
        {
            get;
            set;
        }

  
        [Display(Name = "Country")]
        public long? CountryIdTwo
        {
            get;
            set;
        }


        [Display(Name = "Country")]
        public long? CountryIdThree
        {
            get;
            set;
        }

        [Display(Name = "Country")]
        public long? CountryIdFour
        {
            get;
            set;
        }


        [Display(Name = "State")]
        public long? StateIdOne
        {
            get;
            set;
        }

        [Display(Name = "State")]
        public long? StateIdTwo
        {
            get;
            set;
        }


        [Display(Name = "State")]
        public long? StateIdThree
        {
            get;
            set;
        }


        [Display(Name = "State")]
        public long? StateIdFour
        {
            get;
            set;
        }

        [Display(Name = "City")]
        public string City1 { get; set; }

        [Display(Name = "City")]
        public string City2 { get; set; }

        [Display(Name = "City")]
        public string City3 { get; set; }

        [Display(Name = "City")]
        public string City4 { get; set; }

       
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please provide numeric values.")]
        [Display(Name = "Zip")]
        public string Zip1 { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Please provide numeric values.")]
        [Display(Name = "Zip")]
        public string Zip2 { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Please provide numeric values.")]
        [Display(Name = "Zip")]
        public string Zip3 { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Please provide numeric values.")]
        [Display(Name = "Zip")]
        public string Zip4 { get; set; }

        [Display(Name = "Upload Resume")]
        public HttpPostedFileBase UploadResume1 { get; set; }

        [Display(Name = "Upload Resume")]
        public HttpPostedFileBase UploadResume2 { get; set; }

        [Display(Name = "Upload Resume")]
        public HttpPostedFileBase UploadResume3 { get; set; }
        [Display(Name = "Upload Resume")]
        public HttpPostedFileBase UploadResume4 { get; set; }


        [Display(Name = "Experience")]
        public int? Experience1
        {
            get;
            set;
        }
        [Display(Name = "Experience")]
        public int? Experience2
        {
            get;
            set;
        }
        [Display(Name = "Experience")]
        public int? Experience3
        {
            get;
            set;
        }
        [Display(Name = "Experience")]
        public int? Experience4
        {
            get;
            set;
        }
       

        [Display(Name = "Max")]
        public int? MaximumAge1
        {
            get;
            set;
        }

        [Display(Name = "Min")]
        public int? MinimumAge1
        {
            get;
            set;
        }


        [Display(Name = "Max")]
        public int? MaximumAge2
        {
            get;
            set;
        }

        [Display(Name = "Min")]
        public int? MinimumAge2
        {
            get;
            set;
        }


        [Display(Name = "Max")]
        public int? MaximumAge3
        {
            get;
            set;
        }

        [Display(Name = "Min")]
        public int? MinimumAge3
        {
            get;
            set;
        }

        [Display(Name = "Max")]
        public int? MaximumAge4
        {
            get;
            set;
        }

        [Display(Name = "Min")]
        public int? MinimumAge4
        {
            get;
            set;
        }
 
        [Display(Name = "Qualification")]
        public long? QualificationId1
        {
            get;
            set;
        }


        [Display(Name = "Qualification")]
        public long? QualificationId2
        {
            get;
            set;
        }


        [Display(Name = "Qualification")]
        public long? QualificationId3
        {
            get;
            set;
        }


        [Display(Name = "Qualification")]
        public long? QualificationId4
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the category list.
        /// </summary>
        public IEnumerable<Specialization> Categories
        {
            get
            {
                return SharedService.Instance.GetSpecialisations();
            }
        }

        /// <summary>
        /// Gets the specialization list.
        /// </summary>
        public IEnumerable<SubSpecialization> Specialisations
        {
            get
            {
                return SharedService.Instance.GetSubSpecialisations(SharedService.Instance.GetDefaultSpecialization().Id);
            }
        }

        /// <summary>
        /// Gets the country list.
        /// </summary>
        public IEnumerable<List> Countries
        {
            get
            {
                return SharedService.Instance.GetCountryList();
            }
        }

        /// <summary>
        /// Gets the state list.
        /// </summary>
        public IEnumerable<List> States
        {
            get
            {
                return SharedService.Instance.GetStatesById(Convert.ToInt64(SharedService.Instance.GetDefaultCountry().Id));
            }
        }


        public IEnumerable<List> EmploymentTypes
        {
            get
            {
                return SharedService.Instance.GetJobTypeList();
            }
        }

        public IEnumerable<List> Qualifications
        {
            get
            {
                return SharedService.Instance.GetQualificationList();
            }
        }

        public SelectList AgeList(int age = 18)
        {
            return new SelectList(SharedService.Instance.GetAgeList(age), "value", "key");
        }

        public SelectList ExperienceList(int exp = 0)
        {
            return new SelectList(SharedService.Instance.GetExperienceList(exp), "value", "key");
        }
    }
}