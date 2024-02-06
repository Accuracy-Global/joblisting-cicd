using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class salescall
    {

        [Required(ErrorMessage = "Provide Company  name!")]
        [Display(Name = "Company Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Assocaite Name is required.")]
        public string associatename { get; set; }

        [Required(ErrorMessage = "Provide mobile number!")]
        [RegularExpression("[0-9]+", ErrorMessage = "Only Numbers are allowed in Mobile Number")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Mobile Number must be Min. 6 and Max. 15 numbers!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Provide your registered email address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(
            @"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Login email should be in this format (abcd123@example.com)")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Provide   name!")]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Time  is required.")]
        public string TimeOfCall { get; set; }
        [Required(ErrorMessage = "Date Of call  is required.")]
        public string DofCall { get; set; }
        
        public string CallBackNo { get; set; }
        public string ActiontobeTaken { get; set; }
        public string Notes { get; set; }
    }
}