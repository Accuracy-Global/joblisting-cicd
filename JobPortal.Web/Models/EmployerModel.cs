using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
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
    public class EmployerModel
    {
        public string SELECT = "--SELECT--";
        public long Id { get; set; }

        [Required(ErrorMessage="Provide company name!")]
        [Display(Name="Company Name")]
        [RegularExpression(@"[ A-Za-z0-9_\-'.,&]*$", ErrorMessage = "Special characters not allowed!")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Provide company address!")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[ A-Za-z0-9\n\r_#\-';.,]*$", ErrorMessage = "Special characters not allowed!")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage="Provide country!")]
        public long CountryId { get; set; }

        [Display(Name = "State")]
        public long? StateId { get; set; }

        [RegularExpression(@"[ A-Za-z-]*$", ErrorMessage = "Only Alpahbets, Hyphen and Space is allowed!")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Provide alphanumeric values.")]
        [DataType(DataType.PostalCode)]
        [Display(Name="Zip")]
        public string Zip { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage="Provide First Name!")]
        [RegularExpression(@"[ A-Za-z.]*$", ErrorMessage = "Only space and dot is allowed!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Provide Last Name!")]
        [RegularExpression(@"[ A-Za-z.]*$", ErrorMessage = "Only space and dot is allowed!")]
        public string LastName { get; set; }

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


        [Required(ErrorMessage = "Provide mobile number!")]
        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number!")]
        [StringLength(15, MinimumLength = 6, ErrorMessage ="Mobile Number must be Min. 6 and Max. 15 numbers!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Mobile
        {
            get;
            set;
        }

        [StringLength(700, MinimumLength=150, ErrorMessage = "Company overview must be min. 150 and max. 700 characters!")]
        [Required(ErrorMessage = "Provide company overview!")]
        [Display(Name = "Overview")]
        [DataType(DataType.MultilineText)]
        public string Overview { get; set; }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage="Provide valid website address (http://www.example.com)!")]
        [Display(Name = "Website")]
        [DataType(DataType.Url)]
        public string Website { get; set; }

        [Display(Name = "Upload your company logo")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Logo { get; set; }

        public string Photo { get; set; }

        public bool IsApproved { get; set; }

        [RegularExpression("[A-Za-z0-9_-]*$", ErrorMessage = "Only alphanumeric and underscore and dash is allowed!")]
        public string PremaLink { get; set; }

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
    
        public EmployerModel()
        {
            this.CountryId = SharedService.Instance.GetDefaultCountry().Id;
        }
        public EmployerModel(long id)
        {
            UserProfile Employer = MemberService.Instance.Get(id);

            this.Id = Employer.UserId;
            this.IsFeatured = Employer.IsFeatured;
            this.Company = Employer.Company;
            this.CategoryId = Employer.CategoryId != null ? Employer.CategoryId.Value : default(int);
            this.FirstName = Employer.FirstName;
            this.LastName = Employer.LastName;
            this.Website = Employer.Website;
            this.Address = Employer.Address;
            this.CountryId = Employer.CountryId != null ? Employer.CountryId.Value : default(long);
            this.StateId = (Employer.StateId != null ? Employer.StateId.Value : default(long));
            this.City = Employer.City;
            this.Zip = Employer.Zip;
            this.PhoneCountryCode = Employer.PhoneCountryCode;
            this.Phone = Employer.Phone;
            this.MobileCountryCode = Employer.MobileCountryCode;
            this.Mobile = Employer.Mobile;
        }

        [Required(ErrorMessage="Provide category!")]
        [Display(Name = "Category")]
        public int CategoryId
        {
            get;
            set;
        }
        //[Required(ErrorMessage = "Please Checked IsFeatured.")]
        [Display(Name = "IsFeatured")]
        public bool IsFeatured
        {
            get;
            set;
        }       
    }

    public class EmployerRequiredModel
    {        
        public long Id { get; set; }
        public string ReturnUrl { get; set; }
        
        [Required(ErrorMessage = "Provide company address!")]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[ A-Za-z0-9\n\r_#\-';.,]*$", ErrorMessage = "Special characters not allowed!")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Provide country!")]
        public long CountryId { get; set; }

        [Display(Name = "State")]
        public long? StateId { get; set; }

        [RegularExpression(@"[ A-Za-z-]*$", ErrorMessage = "Only Alpahbets, Hyphen and Space is allowed!")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Provide alphanumeric values.")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

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


        [Required(ErrorMessage = "Provide mobile number!")]
        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number!")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Mobile Number must be Min. 6 and Max. 15 numbers!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Mobile
        {
            get;
            set;
        }

        [StringLength(700, MinimumLength = 150, ErrorMessage = "Company overview must be min. 150 and max. 700 characters!")]
        [Required(ErrorMessage = "Provide company overview!")]
        [Display(Name = "Overview")]
        [DataType(DataType.MultilineText)]
        public string Overview { get; set; }

        [RegularExpression(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "Provide valid website address (http://www.example.com)!")]
        [Display(Name = "Website")]
        [DataType(DataType.Url)]
        public string Website { get; set; }

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

        public EmployerRequiredModel()
        {
            this.CountryId = SharedService.Instance.GetDefaultCountry().Id;
        }
        public EmployerRequiredModel(long id)
        {
            UserProfile Employer = MemberService.Instance.Get(id);

            this.Id = Employer.UserId;
            this.Overview = Employer.Summary;
            this.Address = Employer.Address;
            this.CountryId = Employer.CountryId != null ? Employer.CountryId.Value : default(long);
            this.StateId = (Employer.StateId != null ? Employer.StateId.Value : default(long));
            this.City = Employer.City;
            this.Zip = Employer.Zip;
            this.PhoneCountryCode = Employer.PhoneCountryCode;
            this.Phone = Employer.Phone;
            this.MobileCountryCode = Employer.MobileCountryCode;
            this.Mobile = Employer.Mobile;
        }
    }
}