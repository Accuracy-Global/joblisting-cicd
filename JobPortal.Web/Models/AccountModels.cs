using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Web.Models
{
    public class RegisterExternalLoginModel
    {
        [Required(ErrorMessage = "Provide your email address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Email Address should be in this format (abcd123@example.com)")]
        public string UserName { get; set; }

        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ProviderUsername { get; set; }

        public string ProviderAccessToken { get; set; }

        [Display(Name = "Company")]
        [StringLength(100, ErrorMessage = "Minimum 5 characters!", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\s\'\,\.\-]*$")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Provide first name!")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Provide last name!")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Provide country!")]
        [Display(Name = "Country")]
        public long CountryId { get; set; }

        public string BDay { get; set; }
        public string BMonth { get; set; }
        public string BYear { get; set; }

        [Required(ErrorMessage = "Select account type")]
        [Display(Name = "Account Type")]
        [Range(4, 18, ErrorMessage = "Select account type")]
        public int AccountType { get; set; }

        [Display(Name = "University")]
        public String University { get; set; }

        [Required(ErrorMessage = "Password cannot be blank!")]
        [StringLength(100, ErrorMessage = "Password Min. 9 numbers & letters mixed!", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(9, ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Provide Confirm Password same as Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Language { get; set; }

        public bool Agreed { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Current password cannot be blank!")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password cannot be blank!")]
        [StringLength(100, ErrorMessage = "Password Min. 9 numbers & letters mixed", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(9, ErrorMessage = "Password Min. 9 numbers & letters mixed")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Provide Confirm Password same as Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Confirm Password Min. 9 numbers & letters mixed!")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeEmailAddressModel
    {
        [Display(Name = "New Login Email")]
        [Required(ErrorMessage = "Provide new email address")]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string NewEmailAddress { get; set; }

        [Display(Name = "Old Login Email")]
        [Required(ErrorMessage = "Provide old email address.")]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string OldEmailAddress { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Provide your registered email address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string LoginUsername { get; set; }

        [Required(ErrorMessage = "Password cannot be blank!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LoginPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string number { get; set; }
        public string otp { get; set; }
        public bool IsexternalLogin { get; set; }
    }

    public class RegisterModel
    {


        [Required(ErrorMessage = "Provide mobile number!")]
        //[RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number")]
        //[StringLength(15, MinimumLength = 6, ErrorMessage = "Mobile Number must be Min. 6 and Max. 15 numbers!")]
        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        public string Partners { get; set; }

        public string Freelancers { get; set; }
        public string Interns { get; set; }

        [Required(ErrorMessage = "Provide your email address!")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Provide first name!")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Provide last name!")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password cannot be blank!")]
        [StringLength(100, ErrorMessage = "Password Min. 9 numbers & letters mixed!!", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(9, ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Provide Confirm Password same as Password!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Confirm Password Min. 9 numbers & letters mixed!")]
        public string ConfirmPassword { get; set; }

        public bool Agreed { get; set; }

        [Required(ErrorMessage = "Select account type")]
        [Display(Name = "Account Type")]
        [Range(4, 18, ErrorMessage = "Select account type")]
        public int AccountType { get; set; }

        [Required(ErrorMessage = "Provide country!")]
        [Display(Name = "Country")]
        public long CountryId { get; set; }

        //[Required(ErrorMessage = "Provide Category!")]
        [Display(Name = "Category")]
        public int Category { get; set; }



        [Display(Name = "Company")]
        [StringLength(100, ErrorMessage = "Minimum 5 characters!", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\s\'\,\.\-]*$")]
        public string Company { get; set; }

        public string University { get; set; }

        public string Education { get; set; }


        [Required(ErrorMessage = "Select Company Name")]
        [Display(Name = "Company Name")]
        public String CompanyName { get; set; }
        [Display(Name = "Recruiter Name")]
        public String RecruiterName { get; set; }
        public String Gender { get; set; }
        public String Language { get; set; }
        public string CurrentEmployerToYear { get; set; }

        [Display(Name = "Universityname")]
        public string Universityname { get; set; }

        public string Title { get; set; }

    }

    public class BasicProfileModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "Company")]
        [StringLength(100, ErrorMessage = "Minimum 5 characters!", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\s\'\,\.\-]*$")]
        public string Company { get; set; }
        public string University { get; set; }
        public string Mobile { get; set; }
        public string Language { get; set; }

        public string Education { get; set; }




        public string CurrentEmployerToYear { get; set; }
        [Required(ErrorMessage = "Provide first name!")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Provide last name!")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string LastName { get; set; }

        public string BDay { get; set; }
        public string BMonth { get; set; }
        public string BYear { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderId { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Provide your registered email address")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Password cannot be blank!")]
        [StringLength(100, ErrorMessage = "Password Min. 9 numbers & letters mixed!", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(9, ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Password Min. 9 numbers & letters mixed!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("Password", ErrorMessage = "Provide Confirm Password same as Password!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{9,}$", ErrorMessage = "Confirm Password Min. 9 numbers & letters mixed!")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
        public string Username { get; set; }
    }
}