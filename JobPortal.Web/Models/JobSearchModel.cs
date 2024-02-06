using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Web.Models
{
    public class JobSearchModel
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Specialization")]
        public int SpecializationId { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Employment Type")]
        public int EmploymentTypeId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "State/City")]
        public string State { get; set; }

        [Display(Name = "Job Qualification")]
        public int QualificationId { get; set; }

        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

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

        public string PageNumber
        {
            get;
            set;
        }
    }
}