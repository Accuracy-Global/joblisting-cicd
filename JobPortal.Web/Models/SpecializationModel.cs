using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class SpecializationModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please provide category name.")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public IEnumerable<Specialization> Categories
        {
            get
            {
                return SharedService.Instance.GetSpecialisations();
            }
        }
    }
}