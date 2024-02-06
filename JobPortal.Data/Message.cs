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
    
    public partial class Message
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public System.DateTime StartTimeStamp { get; set; }
        public Nullable<System.DateTime> EndTimeStamp { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public bool IsDeleted { get; set; }
        public string Content { get; set; }
        public bool Sent { get; set; }
    
        public virtual UserProfile Recipient { get; set; }
        public virtual UserProfile Sender { get; set; }
    }
}
