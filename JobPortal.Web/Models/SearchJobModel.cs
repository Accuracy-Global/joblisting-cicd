using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class SearchJobModel : BaseModel
    {
        public long JobID { get; set; }
        public string CountryName { get; set; }
        [Display(Name = "Category")]
        public Nullable<int> CategoryId { get; set; }

        [Display(Name = "Specialization")]
        public Nullable<Int32> SpecializationId { get; set; }

        [Display(Name = "Country")]
        public Nullable<int> CountryId { get; set; }

        [Display(Name = "State")]
        public Nullable<int> StateId { get; set; }
        
        [Display(Name = "Job Type")]
        public Nullable<int> EmploymentTypeId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Zip")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter only numeric value and length should be 6")]
        public string ZipCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "State/City")]
        public string State { get; set; }

        [Display(Name = "Keywords")]
        public string JobTitle { get; set; }
       
        [Display(Name = "Where")]
        public string Where { get; set; }

        [Display(Name = "Qualification")]
        public Nullable<int> QualificationId { get; set; }

        [Display(Name = "Experience")]
        public string Experience { get; set; }
        //public Nullable<int> Experience { get; set; }


        [Display(Name = "From Date")]
        public Nullable<DateTime> FromDate { get; set; }

        [Display(Name = "To Date")]
        public Nullable<DateTime> ToDate { get; set; }

        public string FromDay { get; set; }
        public string FromMonth { get; set; }
        public string FromYear { get; set; }
        public string ToDay { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public double TotalItems { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalRow { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }
        public int DataSize { get; set; }

        public IEnumerable<List> EmploymentTypes
        {
            get
            {
                return SharedService.Instance.GetJobTypeList();
            }
        }
        
        public SelectList ExperienceList(int exp = 0)
        {
            return new SelectList(SharedService.Instance.GetExperienceList(exp), "value", "key");
        }

        public IEnumerable<Job> SearchJobs
        {
            get;
            set;
        }

#pragma warning disable CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<SearchedJobEntity> Jobs
#pragma warning restore CS0246 // The type or namespace name 'SearchedJobEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            get;
            set;
        }

        public int PageNumber
        {
            get;
            set;
        }
        public int TotalJobs { get; set; }
        public string LoginUserName { get; set; }
    }
}