using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ContactModel
    {
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage="Your Message is required")]
        [Display(Name = "Your Message")]
        public string Message { get; set; }
    }
}