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
    
    public partial class InterviewNote
    {
        public int Id { get; set; }
        public long InterviewId { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public string NoteTaker { get; set; }
    
        public virtual Interview Interview { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}