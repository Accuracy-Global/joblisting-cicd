using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class SearchModel
    {
        [Display(Name = "Name")]
        [RegularExpression(@"[ A-Za-z0-9_.-]*$", ErrorMessage = "Special characters not allowed!")]
        public string FullName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Max. Age")]
        public int? AgeMax { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Min. Age")]
        public int? AgeMin { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

       
        [Display(Name = "Relationship Status")]
        public int? RelationshipStatus { get; set; }
        public bool ShowConnect { get; set; }
        public int PageNumber { get; set; }

        public SearchModel()
        {
            PageNumber = 0;
        }
        public string ButtonText { get; set; }
    }
}