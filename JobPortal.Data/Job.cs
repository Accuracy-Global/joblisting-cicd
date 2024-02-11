//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobPortal.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job
    {
        public Job()
        {
            this.Trackings = new HashSet<Tracking>();
        }
    
        public long Id { get; set; }
        public Nullable<long> EmployerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public System.DateTime PublishedDate { get; set; }
        public System.DateTime ClosingDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string Keywords { get; set; }
        public Nullable<long> CountryId { get; set; }
        public Nullable<long> StateId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public Nullable<bool> IsFeaturedJob { get; set; }
        public bool IsSiteJob { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<bool> IsExpired { get; private set; }
        public Nullable<bool> IsPublished { get; set; }
        public string PermaLink { get; set; }
        public Nullable<byte> MinimumAge { get; set; }
        public Nullable<byte> MaximumAge { get; set; }
        public Nullable<byte> MinimumExperience { get; set; }
        public Nullable<byte> MaximumExperience { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> MinimumSalary { get; set; }
        public Nullable<decimal> MaximumSalary { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<long> EmploymentTypeId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public Nullable<long> QualificationId { get; set; }
        public Nullable<int> Openings { get; set; }
        public bool IsRejected { get; set; }
        public bool IsPostedOnTwitter { get; set; }
        public string NewDescription { get; set; }
        public string NewSummary { get; set; }
        public bool InEditMode { get; set; }
        public Nullable<long> NewEmploymentTypeId { get; set; }
        public Nullable<long> NewQualificationId { get; set; }
        public Nullable<byte> NewMinimumAge { get; set; }
        public Nullable<byte> NewMaximumAge { get; set; }
        public Nullable<byte> NewMinimumExperience { get; set; }
        public Nullable<byte> NewMaximumExperience { get; set; }
        public string NewCurrency { get; set; }
        public Nullable<decimal> NewMinimumSalary { get; set; }
        public Nullable<decimal> NewMaximumSalary { get; set; }
        public string Requirements { get; set; }
        public string NewRequirements { get; set; }
        public bool Distribute { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<int> PackageId { get; set; }
        public string Responsilibies { get; set; }
        public string NewResponsilibies { get; set; }
        public string NoticePeriod { get; set; }
        public string AdharNumber { get; set; }
        public string OptionalSkills { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string CompanyName1 { get; set; }
    
        public virtual List Country { get; set; }
        public virtual UserProfile Employer { get; set; }
        public virtual List JobType { get; set; }
        public virtual List Qualification { get; set; }
        public virtual Specialization Category { get; set; }
        public virtual List State { get; set; }
        public virtual SubSpecialization Specialization { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
    }
}