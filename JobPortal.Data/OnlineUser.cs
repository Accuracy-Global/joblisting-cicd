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
    
    public partial class OnlineUser
    {
        public long UniqueId { get; set; }
        public long Id { get; set; }
        public System.DateTime OnlineSince { get; set; }
        public int Status { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
