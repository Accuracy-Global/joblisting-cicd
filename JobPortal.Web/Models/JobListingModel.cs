using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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
using System.Collections;
using System.Web;

namespace JobPortal.Web.Models
{
    public class JobListingModel
    {
        public JobListingModel()
        {
            CountryId = SharedService.Instance.GetDefaultCountry().Id;
        }

        public JobListingModel(long id)
        {
            var job = JobService.Instance.Get(id);

            Title = job.Title;
            IsFeaturedJob = job.IsFeaturedJob.GetValueOrDefault();
            // CategoryId = job.CategoryId.Value;
            SpecializationId = (int)(job.SpecializationId != null ? job.SpecializationId : default(int));
            CountryId = job.CountryId.Value;
            StateId = job.StateId != null ? job.StateId : default(int);
            City = job.City;
            Zip = job.Zip;


            InEditMode = job.InEditMode;
            IsPublished = job.IsPublished.Value;
            IsRejected = job.IsRejected;
            if (job.IsPublished.Value == true && job.InEditMode == true)
            {
                CompanyName = job.CompanyName1;
                Description = job.NewDescription;
                Summary = job.NewSummary;
                Requirements = job.NewRequirements;
                Responsibilities = job.NewResponsilibies;
                MinimumExperience = job.NewMinimumExperience;
                MaximumExperience = job.NewMaximumExperience;
                SalaryCurrency = job.NewCurrency;
                MinimumSalary = job.NewMinimumSalary;
                MaximumSalary = job.NewMaximumSalary;
                MinimumAge = job.NewMinimumAge;
                MaximumAge = job.NewMaximumAge;
                if (job.NewEmploymentTypeId != null)
                {
                    EmploymentType = job.NewEmploymentTypeId.Value;
                }
                QualificationId = job.NewQualificationId;
            }
            else if (job.IsPublished.Value == true && job.InEditMode == false)
            {
                CompanyName = job.CompanyName1;
                Description = job.Description;
                Summary = job.Summary;
                Requirements = job.Requirements;
                Responsibilities = job.Responsilibies;
                MinimumExperience = job.MinimumExperience;
                MaximumExperience = job.MaximumExperience;
                SalaryCurrency = job.Currency;
                MinimumSalary = job.MinimumSalary;
                MaximumSalary = job.MaximumSalary;
                MinimumAge = job.MinimumAge;
                MaximumAge = job.MaximumAge;
                if (job.EmploymentTypeId != null)
                {
                    EmploymentType = job.EmploymentTypeId.Value;
                }
                QualificationId = job.QualificationId;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == true)
            {
                CompanyName = job.CompanyName1;
                Description = job.NewDescription;
                Summary = job.NewSummary;
                Requirements = job.NewRequirements;
                Responsibilities = job.NewResponsilibies;
                MinimumExperience = job.NewMinimumExperience;
                MaximumExperience = job.NewMaximumExperience;
                SalaryCurrency = job.NewCurrency;
                MinimumSalary = job.NewMinimumSalary;
                MaximumSalary = job.NewMaximumSalary;
                MinimumAge = job.NewMinimumAge;
                MaximumAge = job.NewMaximumAge;
                if (job.NewEmploymentTypeId != null)
                {
                    EmploymentType = job.NewEmploymentTypeId.Value;
                }
                QualificationId = job.NewQualificationId;
            }
            else if (job.IsPublished.Value == false && job.InEditMode == false)
            {
                CompanyName = job.CompanyName1;
                Description = job.Description;
                Summary = job.Summary;
                Requirements = job.Requirements;
                Responsibilities = job.Responsilibies;
                MinimumExperience = job.MinimumExperience;
                MaximumExperience = job.MaximumExperience;
                SalaryCurrency = job.Currency;
                MinimumSalary = job.MinimumSalary;
                MaximumSalary = job.MaximumSalary;
                MinimumAge = job.MinimumAge;
                MaximumAge = job.MaximumAge;
                if (job.EmploymentTypeId != null)
                {
                    EmploymentType = job.EmploymentTypeId.Value;
                }
                QualificationId = job.QualificationId;
            }

        }

        public bool IsPublished { get; set; }
        public bool IsRejected { get; set; }
        public bool InEditMode { get; set; }
        public long Id { get; set; }

        [StringLength(45, MinimumLength = 6, ErrorMessage = "Job title must be min. 6 and max. 45 characters!")]
        [Required(ErrorMessage = "Provide job title min. 6 and max. 45 characters!")]
        [Display(Name = "Job Title")]
        [RegularExpression(@"[ A-Za-z0-9_-]*$", ErrorMessage = "Only A-Z, a-z, 0-9,- and space characters allowed!")]
        public string Title { get; set; }

        [Display(Name = "Is Featured Job")]
        public bool IsFeaturedJob { get; set; }

        [Required(ErrorMessage = "Select country.")]
        [Display(Name = "Job Country")]
        public long CountryId { get; set; }

        [Display(Name = "Job State")]
        public long? StateId { get; set; }

        [Display(Name = "Job City")]
        public string City { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Please provide alphanumeric values.")]
        [MaxLength(15, ErrorMessage = "Provide 15 digits zip code.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Select category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Select specialization.")]
        [Display(Name = "Specialization")]
        public int SpecializationId { get; set; }

        //[Required(ErrorMessage = "Provide Min. Experience!")]
        [Display(Name = "MinimumExperience")]
        public int? MinimumExperience { get; set; }

        //[Required(ErrorMessage = "Provide Max. Experience!")]
        [Display(Name = "Maximum Experience")]
        public int? MaximumExperience { get; set; }

        [Display(Name = "Salary Currency")]
        public string SalaryCurrency { get; set; }

        [Display(Name = "Minimum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public decimal? MinimumSalary { get; set; }

        [Display(Name = "Minimum Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
#pragma warning disable CS0103 // The name 'CompareTypes' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'CompareNumerics' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CompareNumericsAttribute' could not be found (are you missing a using directive or an assembly reference?)
        [CompareNumerics("MinimumSalary", CompareTypes.GreaterThanEquals,
#pragma warning restore CS0246 // The type or namespace name 'CompareNumericsAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CompareNumerics' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'CompareTypes' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'ErrorMessage' could not be found (are you missing a using directive or an assembly reference?)
            ErrorMessage = "Maximum Salary should be greater than equals to minimum sal.")]
#pragma warning restore CS0246 // The type or namespace name 'ErrorMessage' could not be found (are you missing a using directive or an assembly reference?)
        public decimal? MaximumSalary { get; set; }

        [Display(Name = "Max")]
        public int? MaximumAge { get; set; }

        [Display(Name = "Min")]
        public int? MinimumAge { get; set; }

        [Required(ErrorMessage = "Select employment type.")]
        [Display(Name = "Employment Type")]
        public long EmploymentType { get; set; }

        [Display(Name = "Qualification")]
        public long? QualificationId { get; set; }

        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job descrption must be min. 10 and max. 250 characters!")]
        [Required(ErrorMessage = "Provide job description min. 10 and max. 250 characters!")]
        [Display(Name = "Job Description")]
        [AllowHtml]
        public string Description { get; set; }


        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job descrption must be min. 10 and max. 250 characters!")]
        [Required(ErrorMessage = "Provide job description min. 10 and max. 250 characters!")]
        [Display(Name = "skills weight")]
        [AllowHtml]
        public string skillsweight { get; set; }


        [Required(ErrorMessage = "Select ThresholdScore.")]
        [Display(Name = "ThresholdScore")]
        public int ThresholdScore { get; set; }


        [StringLength(100, MinimumLength = 10, ErrorMessage = "Description for Searches must be min. 10 and max. 100 characters!")]
        [Required(ErrorMessage = "Provide description for searches min. 10 and max. 100 characters!")]
        [Display(Name = "Description For Searches")]
        [AllowHtml]
        public string Summary { get; set; }

        [AllowHtml]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Summary  must be min. 10 and max. 2000 characters!")]
        [Required(ErrorMessage = "Provide job Summary  min. 10 and max. 2000 characters!")]
        [Display(Name = "Job Summary ")]
        public string Requirements { get; set; }



        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Requirements must be min. 10 and max. 2000 characters!")]
        [Required(ErrorMessage = "Provide job Requirements min. 10 and max. 2000 characters!")]
        [Display(Name = "Job Requirements")]
        [AllowHtml]
        public string Responsibilities { get; set; }

        public String CompanyName { get; set; }
        // [Required(ErrorMessage = "Please enter Company .")]

        [Display(Name = "About Company")]
        public String AboutCompany { get; set; }

        [Display(Name = "Company Logo")]
        public HttpPostedFileBase CompanyLogos { get; set; }
        public int job_id { get; set; }

        public IEnumerable<Specialization> Categories
        {
            get { return SharedService.Instance.GetSpecialisations(); }
        }

        public IEnumerable<SubSpecialization> Specialisations
        {
            get { return SharedService.Instance.GetSubSpecializationBySPID(Convert.ToInt32(CategoryId)); }
        }

        public IEnumerable<List> Countries
        {
            get { return SharedService.Instance.GetCountryList(); }
        }

        public IEnumerable<List> States
        {
            get { return SharedService.Instance.GetStatesById(Convert.ToInt64(CountryId)); }
        }

        public IEnumerable<List> EmploymentTypes
        {
            get { return SharedService.Instance.GetJobTypeList(); }
        }

        public IEnumerable<List> Qualifications
        {
            get { return SharedService.Instance.GetQualificationList(); }
        }

        public SelectList CurrencySymbols
        {
            get { return new SelectList(SharedService.Instance.GetCurrenciesList(), "USD"); }
        }

        public SelectList AgeList(int age = 18, int id = 0)
        {
            SortedList list = SharedService.Instance.GetAgeList(age);
            if (id == -1)
            {
                list.Remove(75);
            }
            return new SelectList(list, "value", "key");
        }

        public SelectList ExperienceList(int exp = 0)
        {
            return new SelectList(SharedService.Instance.GetExperienceList(exp), "value", "key");
        }

        public int Distribute { get; set; }

        public string ReturnUrl { get; set; }
        public int PackageId { get; set; }
        public string Noticeperiod { get; set; }

        [AllowHtml]
        public string Optionalskills { get; set; }






    }
}
//public enum Noticeperiod1
//{


//    Immediate,
//    _15_days,
//    _30_days,
//    _45_days,
//    _60_days

//}