using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ConnectionModel
    {
        [Display(Name="Group")]
        public int? ContactGroup {get;set;}

        [Display(Name = "Name")]
        [RegularExpression(@"[ A-Za-z0-9_.-]*$", ErrorMessage = "Special characters not allowed!")]
        public string Name { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string FirstName{get;set;}

        [Display(Name="Last Name")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\-][a-zA-Z])?[a-zA-Z]*)*$", ErrorMessage = "Special characters and space not allowed!")]
        public string LastName {get;set;}

        [Display(Name="Email Address")]
        [Required(ErrorMessage="Provide email address!")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
           ErrorMessage = "Email Address should be in this format (abcd123@example.com)")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please provide your gender!")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please provide your relationship status!")]
        [Display(Name = "Relationship Status")]
        public int? RelationshipStatus { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Max. Age")]
        public Nullable<int> AgeMax { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Min. Age")]
        public Nullable<int> AgeMin { get; set; }

    }
}