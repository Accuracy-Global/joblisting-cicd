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
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Models;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class JobseekerProfileModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage="Provide first name.")]
        [Display(Name = "First Name")]
        [RegularExpression(@"[ A-Za-z.]*$", ErrorMessage = "Only space and dot is allowed!")]
        public string FirstName
        {
            get;
            set;
        }

        [Required(ErrorMessage="Provide last name.")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"[ A-Za-z.]*$", ErrorMessage = "Only space and dot is allowed!")]
        public string LastName
        {
            get;
            set;
        }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address
        {
            get;
            set;
        }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Please provide alphanumeric values.")]
        [MaxLength(15, ErrorMessage = "Provide 15 digits zip code.")]
        [Display(Name = "Zip")]
        public string Zip
        {
            get;
            set;
        }

        
        [Display(Name = "Country Code")]
        public string PhoneCountryCode
        {
            get;
            set;
        }

        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Select country code!")]
        [Display(Name = "Country Code")]
        public string MobileCountryCode
        {
            get;
            set;
        }

        public string BirthDate { get; set; }
        public int Age { get; set; }

        [Required(ErrorMessage = "Provide mobile number!")]
        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Mobile Number must be Min. 6 and Max. 15 numbers!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Mobile
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Provide gender!")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Provide relationship status!")]
        [Display(Name = "Relationship Status")]
        public int? RelationshipStatus { get; set; }

        public long? ResumeId
        {
            get;
            set;
        }

        [Display(Name = "Profile Title")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Title must be min. 5 and max. 50 characters!")]      
        [RegularExpression(@"[ A-Za-z0-9_.-]*$", ErrorMessage = "Only space and and special characters (_.-) allowed!")]
        public string Title
        {
            get;
            set;
        }

    
        [Display(Name = "Category")]
        public int? CategoryId
        {
            get;
            set;
        }

        [Display(Name = "Specialization")]
        public int? SpecializationId
        {
            get;
            set;
        }
        [Required(ErrorMessage="Provide country!")]
        [Display(Name = "Country")]
        public long? CountryId
        {
            get;
            set;
        }

        [Display(Name = "State")]
        public long? StateId
        {
            get;
            set;
        }

        public string City { get; set; }

        public int? Experience { get; set; }

        [Display(Name = "Currency")]
        public string CurrentCurrency { get; set; }

        [Display(Name = "Current Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public decimal? CurrentSalary { get; set; }

        [Display(Name = "Currency")]
        public string ExpectedCurrency { get; set; }

        [Display(Name = "Expected Salary")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Provide valid numbers!")]
        public decimal? ExpectedSalary
        {
            get;
            set;
        }
        
        [DataType(DataType.Text)]
        [Display(Name="Current Employer")]
        public string CurrentEmployer { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="Previous Employer")]
        public string PreviousEmployer { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "From Month")]
        public string CurrentEmployerFromMonth { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "From Year")]
        public string CurrentEmployerFromYear { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "To Month")]
        public string CurrentEmployerToMonth { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "To Year")]
        public string CurrentEmployerToYear { get; set; }        
        
        [DataType(DataType.Text)]
        [Display(Name = "From Month")]
        public string PreviousEmployerFromMonth { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "From Year")]
        public string PreviousEmployerFromYear { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "To Month")]
        public string PreviousEmployerToMonth { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "To Year")]
        public string PreviousEmployerToYear { get; set; }

        

        [DataType(DataType.Text)]
        [Display(Name="School/Collage")]
        public string School { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="University")]
        public string University { get; set; }

        [Display(Name = "Highest Qualification")]
        public long? QualificationId { get; set; }

        [AllowHtml]
        [Display(Name = "Summary")]
        //[RegularExpression(@"[ A-Za-z0-9?_(.,')-]*$", ErrorMessage = "Only space and and special characters (?_.-) allowed!")]
        public string Summary
        {
            get;
            set;
        }

        [AllowHtml]
        [Display(Name = "Area Of Expertise")]
        public string AreaOfExpertise
        {
            get;
            set;
        }

        [AllowHtml]
        [Display(Name = "Professional Experience")]
        public string ProfessionalExperience { get; set; }

        [AllowHtml]
        [Display(Name = "Technical Skills")]
        public string TechnicalSkills
        {
            get;
            set;
        }

        [AllowHtml]
        [Display(Name = "Management Skills")]
        public string ManagementSkills
        {
            get;
            set;
        }

        [AllowHtml]
        [Display(Name = "Education & Qualification")]
        public string Education { get; set; }

        [Display(Name = "Upload photo")]
        public HttpPostedFileBase Photo { get; set; }

        public string Avatar { get; set; }

        [Display(Name = "Upload Resume")]
        public HttpPostedFileBase Resume { get; set; }

        [RegularExpression("[A-Za-z0-9_-]*$",ErrorMessage="Only alphanumeric and underscore and dash is allowed!")]
        public string PremaLink { get; set; }

        [AllowHtml]
        [Display(Name = "Professional Certification")]
        public string ProfessionalCertification { get; set; }

        [AllowHtml]
        [Display(Name = "Professional Affiliation")]
        public string ProfessionalAffiliation { get; set; }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid facebook page!")]
        [Display(Name = "Facebook Page")]
        public string Facebook
        {
            get;
            set;
        }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid twitter page!")]
        [Display(Name = "Twitter Page")]
        public string Twitter
        {
            get;
            set;
        }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid linkedin page!")]
        [Display(Name = "LinkedIn Page")]
        public string LinkedIn
        {
            get;
            set;
        }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid googleplus page!")]
        [Display(Name = "GooglePlus Page")]
        public string GooglePlus
        {
            get;
            set;
        }
        public IEnumerable<JobPortal.Data.Specialization> Categories
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
                return SharedService.Instance.GetSubSpecializationBySPID(Convert.ToInt32(this.CategoryId));
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'DialingEntity' could not be found (are you missing a using directive or an assembly reference?)
        public List<DialingEntity> CountryWithCodes
#pragma warning restore CS0246 // The type or namespace name 'DialingEntity' could not be found (are you missing a using directive or an assembly reference?)
        {
            get
            {
                List<List> countrylist = SharedService.Instance.GetCountryList();
                List<DialingEntity> countries = new List<DialingEntity>();
                foreach (var item in countrylist)
                {
                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        DialingEntity entity = new DialingEntity()
                        {
                            Key = string.Format("{0} ({1})", item.Text, item.Code),
                            Value = string.Format("+{0}", item.Code)
                        };
                        countries.Add(entity);
                    }
                }                
                return countries;
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
                return SharedService.Instance.GetStatesById(Convert.ToInt64(this.CountryId));
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
                return SharedService.Instance.GetHighestQualificationList();
            }
        }
        public IEnumerable<List> EmploymentTypes
        {
            get
            {
                return SharedService.Instance.GetJobTypeList();
            }
        }

       
        public SelectList CurrencySymbols
        {
            get
            {
                return new SelectList(SharedService.Instance.GetCurrenciesList(), "USD");
            }
        }

        public string Username
        {
            get;
            set;
        }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid website address (http://www.example.com)!")]
        [Display(Name = "Website")]
        [DataType(DataType.Url)]
        public string Website { get; set; }


        public string BDay { get; set; }
        public string BMonth { get; set; }

        [Required(ErrorMessage="Provide birth year!")]
        public string BYear { get; set; }
        public string ReturnUrl { get; set; }
        public string Type { get; set; }
    }
}