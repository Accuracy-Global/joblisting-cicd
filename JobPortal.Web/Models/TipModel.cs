using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class TipModel
    {
        public long Id { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Provide Type")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Provide Subject!")]
        [RegularExpression(@"[ A-Za-z0-9(_)'?,.-]*$", ErrorMessage = "Special characters not allowed!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Provide Body!")]
        [Display(Name = "Body")]
        [AllowHtml]
        public string Body { get; set; }
    }
}