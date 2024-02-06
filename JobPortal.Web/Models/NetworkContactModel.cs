using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class NetworkContactModel
    {
        public long Id { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string EmailAddress { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public bool IsValid { get; set; }
        public bool Connected { get; set; }
    }
}