using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)

namespace JobPortal.Web.Models
{
    public class SeoMainModel : BaseModel
    {
        [Display(Name = "Page Title")]
        [Required]
        [StringLength(55)]
        [DataType(DataType.Text)]
        public string PageTitle { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Page Meta Keyword")]
        public string PageMetaKeyword { get; set; }

        [Display(Name = "Page Meta Description")]
        [Required]
        [DataType(DataType.Text)]
        public string PageMetaDescription { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<Specialization> Categories
        {
            get { return SharedService.Instance.GetSpecialisations(); }
        }

        public int SpecializationId { get; set; }

        public IEnumerable<SubSpecialization> Specialisations
        {
            get
            {
                if ((CategoryId == 0) && (SpecializationId == 0))
                {
                    PageTitle =
                        SharedService.Instance.GetContentSpecialization(
                            SharedService.Instance.GetDefaultSpecialization().Id).Title;
                    PageMetaKeyword =
                        SharedService.Instance.GetContentSpecialization(
                            SharedService.Instance.GetDefaultSpecialization().Id).Keywords;
                    PageMetaDescription =
                        SharedService.Instance.GetContentSpecialization(
                            SharedService.Instance.GetDefaultSpecialization().Id).Description;
                }
                if ((CategoryId > 0) && (SpecializationId == 0))
                {
                    PageTitle = SharedService.Instance.GetContentSpecialization(CategoryId).Title;
                    PageMetaKeyword = SharedService.Instance.GetContentSpecialization(CategoryId).Keywords;
                    PageMetaDescription = SharedService.Instance.GetContentSpecialization(CategoryId).Description;
                }
                if ((CategoryId > 0) && (SpecializationId > 0))
                {
                    PageTitle = SharedService.Instance.GetContentSubSpecialization(SpecializationId).Title;
                    PageMetaKeyword = SharedService.Instance.GetContentSubSpecialization(SpecializationId).Keywords;
                    PageMetaDescription =
                        SharedService.Instance.GetContentSubSpecialization(SpecializationId).Description;
                }
                return SharedService.Instance.GetSubSpecialisations(SharedService.Instance.GetDefaultSpecialization().Id);
            }
        }
    }
}