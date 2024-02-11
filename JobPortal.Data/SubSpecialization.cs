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
    
    public partial class SubSpecialization
    {
        public SubSpecialization()
        {
            this.Resumes = new HashSet<Resume>();
            this.TrackingDetails = new HashSet<TrackingDetail>();
            this.UserProfiles = new HashSet<UserProfile>();
            this.Jobs = new HashSet<Job>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Resume> Resumes { get; set; }
        public virtual ICollection<TrackingDetail> TrackingDetails { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}