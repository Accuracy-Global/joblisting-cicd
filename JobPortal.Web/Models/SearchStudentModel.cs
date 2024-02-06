using Hangfire.Common;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class SearchStudentModel : BaseModel
    {

        [Display(Name = "UserName")]
        public Nullable<int> UserId { get; set; }


        [Display(Name = "Country")]
        public Nullable<int> CountryId { get; set; }

        [Display(Name = "State")]
        public Nullable<int> StateId { get; set; }



        [DataType(DataType.Text)]
        [Display(Name = "Zip")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter only numeric value and length should be 6")]
        public string ZipCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "State/City")]
        public string State { get; set; }

        [Display(Name = "Keywords")]
        public string student { get; set; }

        [Display(Name = "Where")]
        public string Where { get; set; }




        //public IEnumerable<List> EmploymentTypes
        //{
        //    get
        //    {
        //        return SharedService.Instance.GetJobTypeList();
        //    }
        //}

        //public SelectList ExperienceList(int exp = 0)
        //{
        //    return new SelectList(SharedService.Instance.GetExperienceList(exp), "value", "key");
        //}

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
        //public int TotalJobs { get; set; }
        //public string LoginUserName { get; set; }

    }
}