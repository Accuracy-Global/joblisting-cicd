using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortal.Web.Models
{
    public class OtherContentsModel:BaseModel
    {



        [Required(ErrorMessage = "Please select category.")]
        [Display(Name = "Category")]
        public int CategoryId
        {
            get;
            set;
        }
        public int Id
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please select specialization.")]
        [Display(Name = "Specialization")]
        public int SpecializationId
        {
            get;
            set;
        }
        public string PageDescription
        {
            get;
            set;
        }
        public IEnumerable<Specialization> Categories
        {
            get
            {
                return SharedService.Instance.GetSpecialisations();
            }
        }

        public IEnumerable<SubSpecialization> Specialisations
        {
            get
            {
                return SharedService.Instance.GetSubSpecialisations(SharedService.Instance.GetDefaultSpecialization().Id);
            }
        }
        public string getcontent
        {
            get
            {
                if (this.CategoryId == 0)
                {
                    //int id = SharedService.Instance.GetDefaultSpecialization().Id;
                   // string abc = JobPortal.Library.Utility.StringTool.RemoveHtmlTags(SharedService.Instance.GetContentSpecialization(SharedService.Instance.GetDefaultSpecialization().Id).Content.ToString());
                    this.PageDescription = SharedService.Instance.GetContentSpecialization(SharedService.Instance.GetDefaultSpecialization().Id).Content.ToString();
                    return  SharedService.Instance.GetContentSpecialization(SharedService.Instance.GetDefaultSpecialization().Id).Content.ToString();
                   
                }
                else
                {
                    return  SharedService.Instance.GetContentSpecialization(this.CategoryId).Content.ToString();
                }
            }
            
        }
    }
}