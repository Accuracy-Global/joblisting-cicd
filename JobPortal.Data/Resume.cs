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
    
    public partial class Resume
    {
        public Resume()
        {
            this.Trackings = new HashSet<Tracking>();
        }
    
        public long Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public Nullable<long> CountryId { get; set; }
        public Nullable<long> StateId { get; set; }
        public string City { get; set; }
        public Nullable<byte> Experience { get; set; }
        public Nullable<byte> MinimumAge { get; set; }
        public Nullable<byte> MaximumAge { get; set; }
        public Nullable<decimal> DrawingSalary { get; set; }
        public Nullable<decimal> ExpectedSalary { get; set; }
        public string Summary { get; set; }
        public string ProfessionalExperience { get; set; }
        public string Education { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Content { get; set; }
        public Nullable<long> QualificationId { get; set; }
        public string ProfessionalCertification { get; set; }
        public string ProfessionalAffiliation { get; set; }
        public string Zip { get; set; }
        public Nullable<long> EmploymentTypeId { get; set; }
        public string CurrentCurrency { get; set; }
        public string ExpectedCurrency { get; set; }
        public string TechnicalSkills { get; set; }
        public string ManagementSkills { get; set; }
        public string FileName { get; set; }
        public string AreaOfExpertise { get; set; }
    
        public virtual List Country { get; set; }
        public virtual List State { get; set; }
        public virtual Specialization Category { get; set; }
        public virtual SubSpecialization SubSpecialization { get; set; }
        public virtual List Qualification { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
    }
}
