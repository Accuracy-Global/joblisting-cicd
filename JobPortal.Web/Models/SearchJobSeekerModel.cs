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
    public class SearchJobSeekerModel :  BaseModel
    {
    
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string ProfileTitle { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public int? Type { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Where")]
        public string Where { get; set; }


        [Display(Name = "Category")]
        public Nullable<int> CategoryId { get; set; }

        [Display(Name = "Specialization")]
        public Nullable<int> SpecializationId { get; set; }

        [Display(Name = "Country")]
        public Nullable<long> CountryId { get; set; }

        [Display(Name = "State")]
        public Nullable<long> StateId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Max. Age")]
        public Nullable<int> AgeMax { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Min. Age")]
        public Nullable<int> AgeMin { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Experience")]
        public Nullable<int> Experience { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Highest Qualification")]
        public Nullable<int> QualificationId { get; set; }

        [DataType(DataType.Currency,ErrorMessage="Minimum Salary must be in numbers.")]
        [Display(Name = "Minimum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public Nullable<decimal> SalaryMin { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Maximum Salary must be in numbers.")]
        [Display(Name = "Maximum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public Nullable<decimal> SalaryMax { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Display(Name = "Age From")]
        [RegularExpression("^[0-9]*", ErrorMessage = "Provide numbers only!")]
        public int? AgeFrom { get; set; }

        [Display(Name = "Age To")]
        [RegularExpression("^[0-9]*", ErrorMessage = "Provide numbers only!")]
        public int? AgeTo { get; set; }

        [Display(Name = "Relationship")]
        public int? Relationship { get; set; }

        public IEnumerable<List> Experiences
        {
            get
            {
                List<List> lstExperiences = new List<List>();
                for (int i = 0; i <= 60; i++)
                {
                    lstExperiences.Add(new List() { Id = i, Text = i.ToString() });
                }
                return lstExperiences;
            }
        }

        public SelectList AgeList(int age = 18)
        {
            return new SelectList(SharedService.Instance.GetAgeList(age), "value", "key");
        }

        public IEnumerable<UserProfile> SearchJobSeekers
        {
            get;
            set;
        }

#pragma warning disable CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        public IEnumerable<PeopleEntity> ModelList
#pragma warning restore CS0246 // The type or namespace name 'PeopleEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            get;
            set;
        }
        public int TotalRecords { get; set; }
        public int serachType { get; set; }
        public string Username { get; set; }
    }

    public class FilterJobseeker : BaseModel
    {

        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string ProfileTitle { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public int? Type { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Where")]
        public string Where { get; set; }


        [Display(Name = "Category")]
        public Nullable<int> CategoryId { get; set; }

        [Display(Name = "Specialization")]
        public Nullable<int> SpecializationId { get; set; }

        [Display(Name = "Country")]
        public Nullable<long> CountryId { get; set; }

        [Display(Name = "State")]
        public Nullable<long> StateId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Max. Age")]
        public Nullable<int> AgeMax { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Min. Age")]
        public Nullable<int> AgeMin { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Experience")]
        public Nullable<int> Experience { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Highest Qualification")]
        public Nullable<int> QualificationId { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Minimum Salary must be in numbers.")]
        [Display(Name = "Minimum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public Nullable<decimal> SalaryMin { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Maximum Salary must be in numbers.")]
        [Display(Name = "Maximum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public Nullable<decimal> SalaryMax { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Display(Name = "Age From")]
        [RegularExpression("^[0-9]*", ErrorMessage = "Provide numbers only!")]
        public int? AgeFrom { get; set; }

        [Display(Name = "Age To")]
        [RegularExpression("^[0-9]*", ErrorMessage = "Provide numbers only!")]
        public int? AgeTo { get; set; }

        [Display(Name = "Relationship")]
        public int? Relationship { get; set; }       
        public int TotalRecords { get; set; }
        public int serachType { get; set; }
        public string Username { get; set; }
    }
}