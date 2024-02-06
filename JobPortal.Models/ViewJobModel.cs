using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Models
{
    public class ViewJobModel
    {
        public long EmployerId { get; set; }
        public string EmployerLink { get; set; }
        public string CompanyName { get; set; }
        public string CompanySummary { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public int? MinimumAge { get; set; }
        public int? MaximumAge { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public string Currency { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public int? EmployementTypeId { get; set; }
        public string JobType { get; set; }
        public int? Qualification { get; set; }
        public string QualificationName { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Requirements { get; set; }
        public string Responsibilities { get; set; }
        public string JobLink { get; set; }

        public string NewDescription { get; set; }
        public string NewSummary { get; set; }
        public Nullable<long> NewEmploymentTypeId { get; set; }
        public string NewJobType { get; set; }
        public Nullable<long> NewQualificationId { get; set; }
        public string NewQualification { get; set; }
        public Nullable<byte> NewMinimumAge { get; set; }
        public Nullable<byte> NewMaximumAge { get; set; }
        public Nullable<byte> NewMinimumExperience { get; set; }
        public Nullable<byte> NewMaximumExperience { get; set; }
        public string NewCurrency { get; set; }
        public Nullable<decimal> NewMinimumSalary { get; set; }
        public Nullable<decimal> NewMaximumSalary { get; set; }
        public string NewRequirements { get; set; }
        public string NewResponsibilities { get; set; }

        public DateTime PublishedDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Dateline { get; set; }
        public string Region { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRejected { get; set; }
        public bool IsExpired { get; set; }
        public bool IsDeleted { get; set; }
        public bool InEditMode { get; set; }
        public bool IsPaid { get; set; }
        public int? PackageId { get; set; }
        public DateTime? PromoteDate { get; set; }
        public bool ShowActionBar { get; set; }

        public int IsBlockedByMe { get; set; }
        public int IsBlockedBySomeone { get; set; }
        public int InvitedByMe { get; set; }
        public int InvitedBySomeone { get; set; }
        public bool IsConnected { get; set; }
        public long? Connected { get; set; }
        public int Messages { get; set; }
    }
}
