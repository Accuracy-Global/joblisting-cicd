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
    public class UserModel
    {
        public string SELECT = "--- Select ---";

        public long Id { get; set; }
        public string Logo { get; set; }
        [Required(ErrorMessage="Please provide your First Name.")]
        [Display(Name="First Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide your First Name.")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide your Address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select your Country.")]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "State")]
        public Nullable<long> StateId { get; set; }

        [Required(ErrorMessage = "Please provide your City.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please provide your Zip")]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Please provide your Phone.")]
        [Display(Name="Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please provide your Email Address.")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string Email { get; set; }

        [Display(Name = "Upload your photo.")]
        public HttpPostedFileBase Photo { get; set; }

        public List<List> Countries
        {
            get
            {
                return SharedService.Instance.GetCountryList();
            }
        }

        public List<List> States
        {
            get
            {
                return SharedService.Instance.GetStatesById(Convert.ToInt64(this.CountryId));
            }
        }
        public UserModel() { }
        public UserModel(long id)
        {
            UserProfile user = MemberService.Instance.Get(id);

            this.Id = user.UserId;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Address = user.Address;
            this.CountryId = user.CountryId != null ? user.CountryId.Value : default(long);
            this.StateId = user.StateId != null ? user.StateId : default(long);
            this.City = user.City;
            this.Zip = user.Zip;
            this.Phone = user.Phone;
            this.Email = user.Username;
        }
        public UserModel(string Username)
        {
            UserProfile user = MemberService.Instance.Get(Username);

            this.Id = user.UserId;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Address = user.Address;
            this.CountryId = user.CountryId != null ? user.CountryId.Value : default(long);
            this.StateId = user.StateId != null ? user.StateId : default(long);
            this.City = user.City;
            this.Zip = user.Zip;
            this.Phone = user.Phone;
            this.Email = user.Username;
        }
    }
}