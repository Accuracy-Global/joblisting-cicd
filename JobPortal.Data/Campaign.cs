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
    
    public partial class Campaign
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public int CountryId { get; set; }
        public int CategoryId { get; set; }
        public string Username { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public bool Sent { get; set; }
    }
}
