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
    public class SubAdminModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company")]
        public string Company
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Username
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool AgreeCheck { get; set; }
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
        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Please provide alphanumeric values.")]
        [Display(Name = "Zip")]
        public string Zip
        {
            get;
            set;
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
         public SubAdminModel()
        {
            //this.Id = SharedService.Instance.GetDefaultSpecialization().Id;
            SubSpecialization subSpecialization = SharedService.Instance.GetDefaultSubSpecialization();
           
            this.CountryId = SharedService.Instance.GetDefaultCountry().Id;          
        }
         public SubAdminModel(long id)
        {
            UserProfile SubAdmin = MemberService.Instance.Get(id);

            //this.Id = SubAdmin.Id;
            this.Username = Username;
            this.Company = SubAdmin.Company;
            //this.FirstName =SubAdmin.FirstName;
            //this.LastName = SubAdmin.LastName;
            //this.LogoName =SubAdmin.LogoName;
            //this.Website =SubAdmin.Website;
            //this.Address =SubAdmin.Address;
           
            this.CountryId = SubAdmin.CountryId;
            this.StateId = SubAdmin.StateId;
            this.City = SubAdmin.City;
            this.Zip = SubAdmin.Zip;
            //this. =SubAdmin.Telephone;
            //this.Fax =SubAdmin.Fax;
             //this.AlternateEmail = SubAdmin.AlternateEmail;
             //this.IsRecruiter = SubAdmin.IsRecruiter;
             //this.DateCreated = SubAdmin.DateCreated;
             //this.DateUpdated = SubAdmin.DateUpdated;
             //this.CreatedBy = SubAdmin.CreatedBy;
             //this.UpdatedBy = SubAdmin.UpdatedBy;
          
             
        }
         [Required(ErrorMessage = "Please select category.")]
         [Display(Name = "Category")]
         public int CategoryId
         {
             get;
             set;
         }
         public IEnumerable<Specialization> Categories
         {
             get
             {
                 return SharedService.Instance.GetSpecialisations();
             }
         }


    }
}