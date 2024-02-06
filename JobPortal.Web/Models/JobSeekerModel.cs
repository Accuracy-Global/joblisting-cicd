using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class JobSeekerModel
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Email Address")] 
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$", ErrorMessage = "Invalid Email Address")]
       
        public string EmailAddress
        {
            get;
            set;
        }

        [Display(Name = "Alternate Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$", ErrorMessage = "Invalid Email Address")]
        
        public string AlternateEmailAddress
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Home Address")]
        public string Address
        {
            get;
            set;
        }

        [Required]
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

        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City
        {
            get;
            set;
        }

        [Required]
        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Enter only alphanumeric value and length must be 6")]
        [Display(Name = "Zip")]
        public string Zip
        {
            get;
            set;
        }

        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Telephone
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please provide your birth date.")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Upload your photo.")]
        public HttpPostedFileBase Photo { get; set; }

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
        public JobSeekerModel()
        {
            this.Id = SharedService.Instance.GetDefaultSpecialization().Id;
            SubSpecialization subSpecialization = SharedService.Instance.GetDefaultSubSpecialization();

            this.CountryId = SharedService.Instance.GetDefaultCountry().Id;
        }
        public JobSeekerModel(long id)
        {
            UserProfile jobseeker = MemberService.Instance.Get(id);

            this.Id = jobseeker.UserId;

            this.FirstName = jobseeker.FirstName;
            this.LastName = jobseeker.LastName;

            this.Address = jobseeker.Address;

            this.CountryId = jobseeker.CountryId;
            this.StateId = jobseeker.StateId;
            this.City = jobseeker.City;
            this.Zip = jobseeker.Zip;
            this.Telephone = jobseeker.Phone;
            //this.IsPause = jobseeker.IsPause;


        }

    }
}