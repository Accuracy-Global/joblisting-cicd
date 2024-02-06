using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class EmailJobModel
    {
        /// <summary>
        /// Job Id
        /// </summary>
        public long Id { get; set; }
        public string Title { get; set; }

        [Required(ErrorMessage="Provide Sender's Name!")]
        [Display(Name = "Sender Name")]
        [RegularExpression(@"[ A-Za-z_'.-]*$", ErrorMessage = "Special characters not allowed!")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Provide Sender's Email Address!")]
        [Display(Name = "Sender Email Address")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*?\.[a-zA-Z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$",
            ErrorMessage = "Email Address should be in this format (abcd123@example.com)")]
        public string SenderEmailAddress { get; set; }

        [Display(Name = "Friend's Email Address")]
        [Required(ErrorMessage = "Provide maximum 10 emails, separate each email by comma")]
        [RegularExpression(@"^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$", ErrorMessage = "Provide valid email addresses!")]
        public string FriendEmails { get; set; }
    }
}