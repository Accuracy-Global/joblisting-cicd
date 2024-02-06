using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class BlockEmployerModel
    {
        [Display(Name="Status")]
        public string Status { get; set; }

        [Display(Name = "Search By Name")]
        [RegularExpression(@"[ A-Za-z0-9_.-]*$", ErrorMessage = "Special characters not allowed!")]
        public string Search { get; set; }
    }
}