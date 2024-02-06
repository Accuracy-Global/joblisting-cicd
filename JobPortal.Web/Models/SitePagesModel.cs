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
    public class SitePagesModel
    {
         public SitePagesModel()
        {
            this.Id = SharedService.Instance.GetDefaultSpecialization().Id;
            SubSpecialization subSpecialization = SharedService.Instance.GetDefaultSubSpecialization();
           
                   
        }
        public SitePagesModel(int id)
        {
            JobPortalEntities context = new JobPortalEntities();
            SitePage Sitejobs = context.SitePages.Find(id);
            this.Id = Sitejobs.Id;
            this.PageTitle =Sitejobs.PageTitle ;
            this.PageKeywords = Sitejobs.PageKeywords;
            this.PageDescription = Sitejobs.PageDescription;
            this.PageContent = Sitejobs.PageContent;
            this.PageId = Sitejobs.PageId;
            this.CreateDate = Sitejobs.CreateDate;
            this.ModifyDate = Sitejobs.ModifyDate.Value;
           // this.CreateBy = Convert.ToDateTime(Sitejobs.CreateBy);
            this.ModifyBy = Sitejobs.ModifyBy;
          
             
        }
        public int Id  { get; set; }
        public string PageUrl { get; set; }
        public string PageTitle { get; set; }
        public string PageKeywords { get; set; }
        public string PageDescription { get; set; }
        public DateTime CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsDeleted { get; set; }
        public string PageContent { get; set; }
        public string PageText { get; set; }
        public string PageId { get; set; }
        

        /// <summary>
        /// Gets Employer by Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Employer</returns>
      
    }
}