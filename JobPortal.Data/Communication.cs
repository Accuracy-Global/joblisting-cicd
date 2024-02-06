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
    
    public partial class Communication
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public bool Unread { get; set; }
        public bool IsSent { get; set; }
        public bool IsReply { get; set; }
        public bool IsInitial { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public long UserId { get; set; }
    
        public virtual UserProfile Receiver { get; set; }
        public virtual UserProfile Sender { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
