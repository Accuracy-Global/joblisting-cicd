using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Web.Models
{
    public class ApplicationListModel
    {
        public long JobId { get; set; }
        public string Title { get; set; }
        public string PermaLink { get; set; }
        public int Counts { get; set; }

        public string MinimumExperiance { get; set; }
    }
}