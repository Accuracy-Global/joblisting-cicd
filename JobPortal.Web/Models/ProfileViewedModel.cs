using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ProfileViewedModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime DateViewed { get; set; }

        public int Views { get; set; }
    }
}