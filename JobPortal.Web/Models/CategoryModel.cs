using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Display(Name="Name")]
        [Required(ErrorMessage="Please provide category name.")]
        public string Name { get; set; }
        
        [Display(Name = "Title")]
        public string Title { get; set; }
        
        [Display(Name = "Keywords")]
        public string Keywords { get; set; }
       
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}