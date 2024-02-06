using JobPortal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class AdminUserSignUpModel:UserProfile
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company")]
        public new string Company
        {
            get
            {
                return base.Company;
            }
            set 
            { 
                base.Company = value; 
            }
        }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public new string Username
        {
            get
            {
                return base.Username;
            }
            set
            {
                base.Username = value;
            }
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
    }

    }
