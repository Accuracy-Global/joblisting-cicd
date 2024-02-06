using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class LinkedINVM
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }

    public class GoogleProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string varified_Email { get; set; }
        public string MobilePhone { get; set; }
        public string Locale { get; set; }
    }
}