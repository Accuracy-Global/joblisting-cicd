using JobPortal.Data;
#pragma warning disable CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Domain;
#pragma warning restore CS0234 // The type or namespace name 'Domain' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System;
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Enumerators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using JobPortal.Library.Validators;
#pragma warning restore CS0234 // The type or namespace name 'Library' does not exist in the namespace 'JobPortal' (are you missing an assembly reference?)
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace JobPortal.Web.Models
{
    public class NewsLetterModel
    {
        public int Id { get; set; }
        public string Frequency { get; set; }

        public string Type { get; set; }
        public long CountryId { get; set; }

        [Display(Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public string Content { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsSent { get; set; }
        public int ContactTypeId { get; set; }

        public NewsLetterModel()
        {
            this.Id = SharedService.Instance.GetDefaultSpecialization().Id;
        }

        public NewsLetterModel(long id)
        {
            using (JobPortalEntities context = new JobPortalEntities())
            {
                DataHelper dataHelper = new DataHelper(context);
                Newsletter newsletter = dataHelper.GetSingle<Newsletter>(id);
                if (newsletter != null)
                {
                    this.Id = newsletter.Id;
                    this.Frequency = Convert.ToString(newsletter.Frequency);
                    this.Type = Convert.ToString(newsletter.Type);
                    this.CountryId = newsletter.CountryId;
                    this.StartDate = newsletter.StartDate;
                    this.Content = newsletter.Content;
                    this.IsDeleted = newsletter.IsDeleted;
                    this.DateCreated = newsletter.DateCreated;
                    this.CreatedBy = newsletter.CreatedBy;
                    this.DateUpdated = newsletter.DateUpdated;
                    this.UpdatedBy = newsletter.UpdatedBy;
                    this.IsSent = newsletter.IsSent;
                }
            }
        }

        public IEnumerable<List> Countries
        {
            get
            {
                return SharedService.Instance.GetCountryList();
            }
        }

        public IEnumerable<FrequencyType> Frequencies
        {
            get
            {
                return SharedService.Instance.GetFrequencyList();
            }
        }
    }
}