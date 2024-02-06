using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Web.Models
{
    public class SearchResumeModel :  BaseModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Name { get; set; }
        
        [Display(Name = "Category")]
        public Nullable<int> CategoryId { get; set; }
        
        [Display(Name = "Specialization")]
        public Nullable<int> SpecializationId { get; set; }
        
        [Display(Name = "Country")]
        public Nullable<long> CountryId { get; set; }
        
        [Display(Name = "State")]
        public Nullable<long> StateId { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        public int PageNumber { get; set; }
    }
}